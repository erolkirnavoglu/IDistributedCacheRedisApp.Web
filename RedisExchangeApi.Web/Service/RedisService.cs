using StackExchange.Redis;

namespace RedisExchangeApi.Web.Service
{
    public class RedisService
    {
        private readonly string _redisPort;
        private readonly string _redisHost;
        private IDatabase db;
        private ConnectionMultiplexer _redis;
        public RedisService(IConfiguration configuration)
        {
            _redisHost = configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];
        }
        public void Connect()
        {
            var conString = $"{_redisHost}:{_redisPort}";
            _redis=ConnectionMultiplexer.Connect(conString) ;
        }
        public IDatabase GetDb(int db)
        {
            return _redis.GetDatabase(db) ;
        }
    }
}
