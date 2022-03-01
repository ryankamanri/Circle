using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Microsoft.ML.Data;
using static Microsoft.ML.DataOperationsCatalog;
using MLServer.Models;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace MLServer.Services
{
	public interface IMLService
	{

	}
	public class MLService : IMLService
	{
		private readonly ILogger _logger;

		private MLContext _mlContext;
		public MLService(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<MLService>();
			_logger.LogDebug("Starting The Mathine Learning Service");
			StartMLService();
		}

		/// <summary>
		/// 开始获取用户模型并进行回归分析
		/// </summary>
		private void StartMLService()
		{
			_mlContext = new MLContext();
			var splitDataView = LoadData();
		}

		private async Task<TrainTestData> LoadData()
		{
			// 1. 结构化数据
			//_mlContext.Data.LoadFromEnumerable<DoubleUserData>();
			var redis = await ConnectionMultiplexer.ConnectAsync("localhost");
			var rdb = redis.GetDatabase();
			if(await rdb.StringSetAsync("test_data", "kamanri"))
			{
				var value = await rdb.StringGetAsync("test_data");
				_logger.LogDebug(value);
			}
			return default;
		}
	}
}
