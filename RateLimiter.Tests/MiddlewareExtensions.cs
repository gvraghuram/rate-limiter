using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RateLimiter.DataAccess;
using RateLimiter.LocationService;
using RateLimiter.Middlewares;
using RateLimiter.RulesEngine;

namespace RateLimiter.Tests
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddMyServices(this IServiceCollection services)
        {
            services.UseRateLimiter();
            services.AddSingleton<ILocationService, LocationService.LocationService>();
            services.AddSingleton<IStorageService, InMemoryService>();
            services.AddSingleton(typeof(IConfiguration), Configuration.GetConfiguration);

            return services;
        }

        public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimiterMiddleware>();
        }
    }
}
