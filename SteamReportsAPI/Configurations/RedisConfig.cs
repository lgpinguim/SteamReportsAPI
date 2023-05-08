using StackExchange.Redis;

namespace SteamReportsAPI.Configurations
{
    public static class RedisConfig
    {
        public static void AddRedisConfiguration(this IServiceCollection services)
        {

            var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
            var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT");

            if (services == null) throw new ArgumentNullException(nameof(services));

            //var password = builder.Configuration["Redis:Password"];
            var host = redisHost;
            var port = redisPort;
            var config = new ConfigurationOptions
            {
                EndPoints = { { host!, Convert.ToInt32(port) } },
                ConnectRetry = 3,
                AbortOnConnectFail = false
            };
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = config;
                options.ConnectionMultiplexerFactory = async () =>
                {
                    var connection = await ConnectionMultiplexer.ConnectAsync(config);
                    return connection;
                };
            });

        }
    }
}
