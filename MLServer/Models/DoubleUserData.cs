using System;

namespace MLServer.Models
{
	public class DoubleUserData
	{
		/// <summary>
		/// 用户1
		/// </summary>
		// 基本信息
		public string University { get; set; }
		public string School { get; set; }
		public string Speciality { get; set; }
		public DateTime dateTime { get; set; }
		public string Introduction { get; set; }

		// 标签信息
		public string SelfTags { get; set; }
		public string InterestedTags { get; set; }
		/// 发布的所有帖子的所有标签
		public string PostsTags { get; set; }
		/// <summary>
		/// 用户2
		/// </summary>
		// 基本信息
		public string University2 { get; set; }
		public string School2 { get; set; }
		public string Speciality2 { get; set; }
		public DateTime dateTime2 { get; set; }
		public string Introduction2 { get; set; }

		// 标签信息
		public string SelfTags2 { get; set; }
		public string InterestedTags2 { get; set; }
		/// 发布的所有帖子的所有标签
		public string PostsTags2 { get; set; }


		/// <summary>
		/// 相似度， 由实际用户来往情况决定， 来往越多相似度越高
		/// 这个地方需要由这两位用户是否互关、相互点赞、评论及收藏的帖子等等来决定
		/// </summary>
		public double SimilarityScore { get; set; }

	}

	public class DoubleUserPrediction : DoubleUserData
	{

	}

	
}
