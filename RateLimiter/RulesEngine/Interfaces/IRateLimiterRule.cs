using RateLimiter.Model;

namespace RateLimiter.RulesEngine.Interfaces
{
    public interface IRateLimiterRule
    {
        bool IsEnabled(UserRequest request);
        bool Validate(UserRequest request);
    }
}
