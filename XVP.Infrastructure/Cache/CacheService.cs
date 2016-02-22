namespace XVP.Infrastructure.Shared.Cache
{
    using System;
    using System.Configuration;

    using StackExchange.Redis;

    public static class CacheService
    {
        private static readonly string CacheKey = "xvpredis.redis.cache.windows.net,ssl=true,password=" + ConfigurationManager.AppSettings["AzureRedisCacheKey"];

        private static readonly
            Lazy<ConnectionMultiplexer> LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(CacheKey));

        private static IDatabase cache = Connection.GetDatabase();

        public static IDatabase Cache
        {
            get
            {
                return cache;
            }

            set
            {
                cache = value;
            }
        }

        public static void Flush()
        {
            string configString = CacheKey;
            var options = ConfigurationOptions.Parse(configString);
            options.AllowAdmin = true;
            var conn = ConnectionMultiplexer.Connect(options);
            var endpoints = conn.GetEndPoints();
            var server = conn.GetServer(endpoints[0]);
            server.FlushDatabase();
        }
        
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return LazyConnection.Value;
            }
        }
    }
}
