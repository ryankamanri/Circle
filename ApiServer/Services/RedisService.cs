using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
namespace ApiServer.Services
{
    public class RedisService
    {
        public static class StoredObjectType
        {
            public const int IMAGE = 1;
        }
        
        private ConnectionMultiplexer _redis;

        public RedisService(IConfiguration config)
        {
            var server = config["Redis:Server"];
            _redis = ConnectionMultiplexer.ConnectAsync(server).Result;
        }

        public Task<RedisValue> GetAsync(int storedObjectType, RedisKey key)
        {
            var db = _redis.GetDatabase(storedObjectType, true);
            return db.StringGetAsync(key);
        }

        public Task<bool> SetAsync(int storedObjectType, RedisKey key, RedisValue vaLue)
        {
            var db = _redis.GetDatabase(storedObjectType, true);
            return db.StringSetAsync(key, vaLue);
        }
        
        
    }
}