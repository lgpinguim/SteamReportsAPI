using SteamReportsAPI.Configurations;

namespace SteamReportsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            builder.Services.AddControllers();

            // Setting DBContexts
            builder.Services.AddDatabaseConfiguration(builder.Configuration);

            if (builder.Environment.IsDevelopment())
                builder.Services.AddDistributedMemoryCache();

            else
                //Setting redis
                builder.Services.AddRedisConfiguration();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerConfiguration();

            // .NET Native DI Abstraction
            builder.Services.AddDependencyInjectionConfiguration();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseCors();

            app.UseSwaggerSetup();

            app.Run();
        }
    }
}