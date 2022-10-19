﻿using RateLimiter.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RateLimiter.LocationService
{
    public interface ILocationService
    {
        ClientRegions GetRegionFromIp(IPAddress iPAddress);
    }
}
