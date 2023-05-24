using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Services
{
    public class RedisService
    {
        private readonly RedisService _redisService;

        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public IDatabase database { get; set; }
        public RedisService(string url)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(url);
        }

        public IDatabase GetDb(int databaseIndex)
        {
            return _connectionMultiplexer.GetDatabase(databaseIndex);
        }


    }
}
