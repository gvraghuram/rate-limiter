using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RateLimiter.DataAccess;
using RateLimiter.Middlewares;
using RateLimiter.RulesEngine.Interfaces;

namespace RateLimiter.RulesEngine
{
    public static class RateLimiterInjectionExtensions
    {
        public static IServiceCollection AddRateLimitServices(this IServiceCollection services)
        {
            services.AddSingleton<IRuleEngine, RuleEngine>();
            services.AddSingleton<IRateLimiterRule, RequestsPerTimeRule>();
            services.AddSingleton<IRateLimiterRule, TimeRule>();

            return services;
        }

        public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimiterMiddleware>();
        }
    }
}