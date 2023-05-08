using Microsoft.OpenApi.Models;

namespace SteamReportsAPI.Configurations
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Steam Reports Minimal API",
                    Description = "A minimal API developed for Funcom",
                    Contact = new OpenApiContact { Name = "Luis Gustavo Fernandes Ferreira", Email = "luisgustavofernandesferreira@outlook.com", Url = new Uri("https://github.com/lgpinguim") },
                });

            });
        }

        public static void UseSwaggerSetup(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }
    }
}
