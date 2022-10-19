using RateLimiter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RateLimiter.DataAccess
{
    public class InMemoryService : IStorageService
    {
        private Dictionary<string, ClientRequestCache> _cache = new();

        public ClientRequestCache GetToken(string key)
        {
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }
            
            return null;
        }

        public void SetToken(string key, ClientRequestCache token)
        {
            _cache[key] = token;
        }
    }
}
