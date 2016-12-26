using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using System.Security.Claims;

namespace NetCoreBoot.Common
{
    public class WebHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public WebHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetSession(string key, string value)
        {
            _session.SetString(key, value);
        }

        public string GetSession(string key)
        {
            return _session.GetString(key);
        }

    }
}
