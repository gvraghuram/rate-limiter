using System;

namespace RateLimiter.Model
{
    public class ClientRequestCache
    {
        public DateTime LastRequest { get; set; }
        public int RequestCount { get; set; }
    }
}
