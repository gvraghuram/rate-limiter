using System;

namespace RateLimiter.Model
{
    public class ClientRequest
    {
        public Token Token { get; }
        public Regions Region { get; }
        public DateTime RequestTime { get; }

        public ClientRequest(Token token, Regions region) : this(token, region, DateTime.UtcNow) { }
        public ClientRequest(Token token, Regions region, DateTime requestTime)
        {
            Token = token;
            Region = region;
            RequestTime = requestTime;
        }
    }
}