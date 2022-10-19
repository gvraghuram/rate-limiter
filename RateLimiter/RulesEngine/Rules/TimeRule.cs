using Microsoft.Extensions.Configuration;
using RateLimiter.DataAccess;
using RateLimiter.Model;
using RateLimiter.RulesEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateLimiter.RulesEngine.Rules
{
    public class TimeRule : IRateLimiterRule
    {
        private readonly IStorageService storageService;
        private readonly int timeBetweenRequestsInMs;
        private const string TimeBetweenRequestsInMsVar = "timeBetweenRequestsInMs";

        public TimeRule(IStorageService storageService, IConfiguration configuration)
        {
            this.storageService = storageService;
            timeBetweenRequestsInMs = int.Parse(configuration[TimeBetweenRequestsInMsVar]);
        }

        public bool IsEnabled(ClientRequest request)
        {
            return ClientRegions.EU.Equals(request.Region);
        }

        public bool Validate(ClientRequest request)
        {
            var lastRequest = storageService.GetToken(request.ClientToken.Ip.ToString());

            if (lastRequest == null)
            {
                storageService.SetToken(request.ClientToken.Ip.ToString(), new ClientRequestCache { LastRequest = request.RequestTime });
                return true;
            }

            var result = request.RequestTime - lastRequest.LastRequest > TimeSpan.FromMilliseconds(timeBetweenRequestsInMs);

            if (result)
            {
                lastRequest.LastRequest = request.RequestTime;
                storageService.SetToken(request.ClientToken.Ip.ToString(), lastRequest);
            }

            return result;
        }
    }
}
