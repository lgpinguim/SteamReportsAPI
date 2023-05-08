using Microsoft.Extensions.DependencyInjection;
using SteamReports.Application.Interfaces;
using SteamReports.Application.Services;
using SteamReports.Domain.Interfaces;
using SteamReports.Infra.Data.Context;
using SteamReports.Infra.Data.Repository;

namespace SteamReports.Infra.CrossCutting.IoC
{
    public static class NativeInjectorBootstrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Application
            services.AddScoped<IReviewAppService, ReviewAppService>();

            // Infra - Data
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<ISteamAppRepository, SteamAppRepository>();
            services.AddScoped<SteamReportsContext>();

        }
    }
}
