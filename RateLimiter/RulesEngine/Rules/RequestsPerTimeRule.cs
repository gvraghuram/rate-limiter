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
    public class RequestsPerTimeRule : IRateLimiterRule
    {
        private readonly int maxRequests;
        private readonly int timeSpan;
        private readonly IStorageService storageService;
        private const string MaxRequests = "maxRequestInTs";
        private const string TimeSpanRequest = "timeRequestSpanInMs";


        public RequestsPerTimeRule(IStorageService storageService, IConfiguration configuration)
        {
            this.storageService = storageService;
            maxRequests = int.Parse(configuration[MaxRequests]);
            timeSpan = int.Parse(configuration[TimeSpanRequest]);
        }

        public bool IsEnabled(ClientRequest request)
        {
            return ClientRegions.US.Equals(request.Region);
        }

        public bool Validate(ClientRequest request)
        {
            var lastRequest = storageService.GetToken(request.ClientToken.Ip.ToString());

            if (lastRequest == null)
            {
                storageService.SetToken(request.ClientToken.Ip.ToString(), new ClientRequestCache { LastRequest = request.RequestTime, RequestCount = 1 });
                return true;
            }

            var result = lastRequest.LastRequest.AddMilliseconds(timeSpan) < request.RequestTime;
            if (result)
            {
                lastRequest.LastRequest = request.RequestTime;
                lastRequest.RequestCount = 1;
                storageService.SetToken(request.ClientToken.Ip.ToString(), lastRequest);
                return true;
            }
            if (++lastRequest.RequestCount <= maxRequests)
            {
                storageService.SetToken(request.ClientToken.Ip.ToString(), lastRequest);
                return true;
            }

            return false;
        }
    }
}
