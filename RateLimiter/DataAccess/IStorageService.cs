using RateLimiter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RateLimiter.DataAccess
{
    public interface IStorageService
    {
        UserRequestCache GetToken(string key);
        void SetToken(string key, UserRequestCache token);
    }
}
