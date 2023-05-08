namespace SteamReportsAPI.Configurations
{
    public static class RedisConfig
    {
        public static void AddRedisConfiguration(this IServiceCollection services)
        {
            var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
            var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT");

            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{redisHost}:{redisPort}";
            });
        }
    }
}
