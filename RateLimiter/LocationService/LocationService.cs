using RateLimiter.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RateLimiter.LocationService
{
    public class LocationService : ILocationService
    {
        public ClientRegions GetRegionFromIp(IPAddress iPAddress)
        {
            return ClientRegions.US;
        }
    }
}
