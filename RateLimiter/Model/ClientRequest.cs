using System;

namespace RateLimiter.Model
{
    public class ClientRequest
    {
        public ClientToken ClientToken { get; }
        public ClientRegions Region { get; }
        public DateTime RequestTime { get; }

        public ClientRequest(ClientToken token, ClientRegions region) : this(token, region, DateTime.UtcNow) { }
        public ClientRequest(ClientToken token, ClientRegions region, DateTime requestTime)
        {
            ClientToken = token;
            Region = region;
            RequestTime = requestTime;
        }
    }
}