using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Microsoft.ML.Data;
using static Microsoft.ML.DataOperationsCatalog;
using MLServer.Models;
using StackExchange.Redis;
using System.Threading.Tasks;
using Kamanri.Database;
using Kamanri.Database.Models.Relation;
using Kamanri.Extensions;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using MLServer.Models.BasicModels;
using MLServer.Models.BasicModels.Post;
using MLServer.Models.BasicModels.User;
using MLServer.Extensions;

namespace MLServer.Services
{
	public interface IMLService
	{
		public bool IsReady();
		public ClusterResultDataItem Predict(long userID);
		public IEnumerable<ClusterFeaturesDataItem> GetClusterUsers(ClusterResultDataItem predictResult);
	}
	
	
	public class MLService : IMLService
	{
		class DataLists
		{
			public IList<UserInfo> UserInfoList { get; set; }
			public IList<Tag> TagList { get; set; }
			public IList<Post> PostList { get; set; }
			public IList<Comment> CommentList { get; set; }
			
			public ID_IDList Users_Tags_List { get; set; }
			public ID_IDList Users_Posts_List { get; set; }
			public ID_IDList Users_Comments_List { get; set; }
			public ID_IDList Users_Users_List { get; set; }
			
			public ID_IDList Posts_Tags_List { get; set; }
			
			public ID_IDList Tags_Tags_List { get; set; }
		}

		private readonly ILogger _logger;

		private DatabaseContext _dbc;

		private MLContext _mlContext;

		private DataLists _dataLists = new DataLists();

		private TransformerChain<TransformerChain<ClusteringPredictionTransformer<KMeansModelParameters>>> _model;

		private IEnumerable<ClusterResultDataItem> _clusterResult;

		private const int K_FOLD = 5;

		private const int AVG_CLUSTER_USER_COUNT = 2;

		private Action _Next;

		private bool _isReady = false;

		public bool IsReady() => _isReady;

		public MLService(ILoggerFactory loggerFactory, DatabaseContext dbc)
		{
			_mlContext = new MLContext(1);
			_logger = loggerFactory.CreateLogger<MLService>();
			_dbc = dbc;
			Task.Run(async() =>
			{
				await RunMLServiceOnce();
				_isReady = true;
				Thread.Sleep(10 * 60 * 1000);
			});
		}

		/// <summary>
		/// 开始获取用户模型并进行聚类分析
		/// </summary>
		private async Task RunMLServiceOnce()
		{
			_logger.LogDebug("Starting The Machine Learning Service");
			await LoadData();
			
			// preprocess data
			var preprocessDatas = LoadClusterData().GroupByUserAndTag().AddTagTreeWeight(_dataLists.Tags_Tags_List).ToList();
			// Console.WriteLine(ClusterUserDataItem.HeaderString());
			// foreach (var clusterData in preprocessDatas)
			// {
			// 	Console.WriteLine(clusterData);
			// }
			_logger.LogDebug("Loaded Cluster Data.");
			
			// extract features
			var proceededFeaturesDatas = preprocessDatas.ToClusterFeaturesDataItems().ToList();

			var clusterCount = (int)Math.Floor(((double)proceededFeaturesDatas.Count() / AVG_CLUSTER_USER_COUNT));
			// set the pipeline
			var dataPipeline = _mlContext.Transforms.ProjectToPrincipalComponents(
				outputColumnName: "PCAFeatures",
				inputColumnName: "Features",
				rank: 2
			).Append(_mlContext.Transforms.Categorical.OneHotEncoding(
				outputColumnName: "UserIDKey",
				inputColumnName: "UserID",
				OneHotEncodingEstimator.OutputKind.Indicator
				).Append(_mlContext.Clustering.Trainers.KMeans(
				featureColumnName: "Features",
				numberOfClusters: clusterCount)));
			
			var data = _mlContext.Data.LoadFromEnumerable(proceededFeaturesDatas);
			
			// fitting the model
			_model = dataPipeline.Fit(data);

			_clusterResult = _mlContext.Data.CreateEnumerable<ClusterResultDataItem>(_model.Transform(data), false);
			
			//evaluate model
			// use k-Fold Cross Validation, K = min(5, count)
			_logger.LogDebug("Start To Evaluate Model...");
			// var k = Math.Min(K_FOLD, proceededFeaturesDatas.Count);
			// for (var i = 0; i < k; i++)
			// {
			// 	var count = proceededFeaturesDatas.Count / k;
			// 	var index = i * count;
			// 	var evaluateData = _mlContext.Data.LoadFromEnumerable(proceededFeaturesDatas.GetRange(index, count));
			// 	var evaluatePredictions = _model.Transform(evaluateData);
			// 	var metrics = _mlContext.Clustering.Evaluate(evaluatePredictions, scoreColumnName: "Score", featureColumnName: "Features");
			// 	
			// 	// ReSharper disable once TemplateIsNotCompileTimeConstantProblem
			// 	_logger.LogDebug($"\tEvaluate Model:\n" +
			// 	                 $"\tIndex = {index}, Count = {count}\n" +
			// 	                 $"\t{metrics.AverageDistance} = Average Distance\n" +
			// 	                 $"\t{metrics.DaviesBouldinIndex} = Davies Bouldin Index\n" +
			// 	                 $"\t{metrics.NormalizedMutualInformation} = Normalized Mutual Information\n");
			// }
			
			var evaluatePredictions = _model.Transform(data);
			var metrics = _mlContext.Clustering.Evaluate(evaluatePredictions, scoreColumnName: "Score", featureColumnName: "Features");
				
			// ReSharper disable once TemplateIsNotCompileTimeConstantProblem
			_logger.LogDebug($"\tEvaluate Model:\n" +
			                 $"\t{metrics.AverageDistance} = Average Distance\n" +
			                 $"\t{metrics.DaviesBouldinIndex} = Davies Bouldin Index\n" +
			                 $"\t{metrics.NormalizedMutualInformation} = Normalized Mutual Information\n");
			
			
			// consume model
			

		}

		

		public ClusterResultDataItem Predict(long userID)
		{
			var predictData = new ClusterFeaturesDataItem(LoadAUserClusterData(userID).GroupByUserAndTag().AddTagTreeWeight(_dataLists.Tags_Tags_List));

			var predictList = new List<ClusterFeaturesDataItem>() { predictData };

			var predictDataView = _mlContext.Data.LoadFromEnumerable(predictList);
			
			var transformedDataView = _model.Transform(predictDataView);

			var predictions = _mlContext.Data.CreateEnumerable<ClusterResultDataItem>(transformedDataView, false);

			return predictions.First();
		}

		public IEnumerable<ClusterFeaturesDataItem> GetClusterUsers(ClusterResultDataItem predictResult)
		{
			foreach (var result in _clusterResult)
			{
				if (result.PredictedLabel == predictResult.PredictedLabel)
					yield return result;
			}
		}


		private IEnumerable<ClusterUserDataItem> LoadClusterData()
		{
			var result = new List<ClusterUserDataItem>();
			foreach (var userInfo in _dataLists.UserInfoList)
			{
				result.AddRange(LoadAUserClusterData(userInfo.ID));
			}
			_logger.LogDebug($"Loaded {result.Count} Items");
			return result;
		}

		private IEnumerable<ClusterUserDataItem> LoadAUserClusterData(long userID)
		{
			var result = new List<ClusterUserDataItem>();

			foreach (var tag in _dataLists.TagList)
			{
				if(tag.ID == -1) continue;
				result.AddRange(LoadAClusterData(userID, tag.ID));
			}

			return result;
		}


		



		private IEnumerable<ClusterUserDataItem> LoadAClusterData(long userID, long tagID)
		{
			//Is Self & Interested
			var isSelf = 0;
			var isInterested = 0;
			var user_tag_relation = (from user_tag in _dataLists.Users_Tags_List
				where user_tag.ID == userID && user_tag.ID_2 == tagID
				select user_tag).FirstOrDefault();
			if (user_tag_relation != default)
			{
				var relation = user_tag_relation.Relations as dynamic;
				if (relation != null && relation.Type != null)
				{
					if (relation.Type.Contains("Self"))
					{
						isSelf = 1;
						_logger.LogTrace("Is Self");
					}

					if (relation.Type.Contains("Interested"))
					{
						isInterested = 1;
						_logger.LogTrace("Is Interested");
					}
				}
			}
			
			// Is Owned Post, Like, Collect Or Comment
			

			var postIDList = (
					from post_tag in _dataLists.Posts_Tags_List
					where post_tag.ID_2 == tagID
					select post_tag.ID)
				.Intersect(from user_post in _dataLists.Users_Posts_List
					where user_post.ID == userID 
					select user_post.ID_2);

			foreach (var postID in postIDList)
			{ // for every post
				var isOwned = 0;
				var isLike = 0;
				var isCollect = 0;
				var user_post_relation = (from user_post in _dataLists.Users_Posts_List
					where user_post.ID == userID && user_post.ID_2 == postID
					select user_post).FirstOrDefault();
				if (user_post_relation != default)
				{
					var relation = user_post_relation.Relations as dynamic;
					if (relation != null && relation.Type != null)
					{
						if (relation.Type.Contains("Owned"))
						{
							isOwned = 1;
							_logger.LogTrace("Is Owned");
						}

						if (relation.Type.Contains("Like"))
						{
							isLike = 1;
							_logger.LogTrace("Is Like");
						}
						if (relation.Type.Contains("Collect"))
						{
							isCollect = 1;
							_logger.LogTrace("Is Collect");
						}
						
					} // if (relation != null && relation.Type != null)
				} // if (user_post_relation != default)
				
				var commentIDCount = (
						from comment in _dataLists.CommentList
						where comment.PostID == postID
						select comment.ID)
					.Intersect(from user_comment in _dataLists.Users_Comments_List
						where user_comment.ID == userID 
						select user_comment.ID_2).Count();
				
				// add to a cluster data, which is grouped by every (user,post, tag)
				yield return new ClusterUserDataItem()
				{
					IsPostCollect = isCollect,
					IsPostLike = isLike,
					IsPostOwned = isOwned,
					PostCommentCount = commentIDCount,
					TagID = tagID,
					UserID = userID,
					Count = isCollect * 3 + isLike * 2 + isOwned * 10 + commentIDCount * 3
				};
			} // foreach (var postID in postIDList)
			
			yield return new ClusterUserDataItem()
			{
				IsInterested = isInterested,
				IsSelf = isSelf,
				Count = isInterested * 30 + isSelf * 20,
				TagID = tagID,
				UserID = userID
			};
		}

		private async Task LoadData()
		{
			_logger.LogDebug("Load Data From Database...");
			_dataLists.UserInfoList = await _dbc.SelectAll<UserInfo>();
			_dataLists.PostList = await _dbc.SelectAll<Post>();
			_dataLists.CommentList = await _dbc.SelectAll<Comment>();
			_dataLists.TagList = await _dbc.SelectAll<Tag>();

			_dataLists.Users_Posts_List = await _dbc.SelectAllRelations<User, Post>();
			_dataLists.Users_Comments_List = await _dbc.SelectAllRelations<User, Comment>();
			_dataLists.Users_Tags_List = await _dbc.SelectAllRelations<User, Tag>();
			_dataLists.Users_Users_List = await _dbc.SelectAllRelations<User, User>();
			_dataLists.Posts_Tags_List = await _dbc.SelectAllRelations<Post, Tag>();
			_dataLists.Tags_Tags_List = await _dbc.SelectAllRelations<Tag, Tag>();
			
			// Task.Run(async() =>
			// {
			// 	while (true)
			// 	{
			// 		Thread.Sleep(10 * 60 * 1000);
			// 		_dataLists.UserInfoList = await _dbc.SelectAll<UserInfo>();
			// 		_dataLists.PostList = await _dbc.SelectAll<Post>();
			// 		_dataLists.CommentList = await _dbc.SelectAll<Comment>();
			// 		_dataLists.TagList = await _dbc.SelectAll<Tag>();
			//
			// 		_dataLists.Users_Posts_List = await _dbc.SelectAllRelations<User, Post>();
			// 		_dataLists.Users_Comments_List = await _dbc.SelectAllRelations<User, Comment>();
			// 		_dataLists.Users_Tags_List = await _dbc.SelectAllRelations<User, Tag>();
			// 		_dataLists.Users_Users_List = await _dbc.SelectAllRelations<User, User>();
			// 		_dataLists.Posts_Tags_List = await _dbc.SelectAllRelations<Post, Tag>();
			// 		_logger.LogDebug("Reload Data From Database...");
			// 	}
			// });
		}
		
 
		

		// private async Task<TrainTestData> LoadData()
		// {
		// 	 // 1. 结构化数据
		// 	_mlContext.Data.LoadFromEnumerable<DoubleUserData>();
		// 	 var redis = await ConnectionMultiplexer.ConnectAsync("localhost");
		// 	 var rdb = redis.GetDatabase();
		// 	 if(await rdb.StringSetAsync("test_data", "kamanri"))
		// 	 {
		// 	 	var value = await rdb.StringGetAsync("test_data");
		// 	 	_logger.LogDebug(value);
		// 	 }
		// 	 return default;
		// }
	}
}
