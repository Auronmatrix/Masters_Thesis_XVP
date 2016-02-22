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

    public static ConnectionMultiplexer Connection
    {
        get
        {
            return LazyConnection.Value;
        }
    }
}