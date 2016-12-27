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
        //构造HttpContextAccessor存取器 需手动注册服务
        private readonly IHttpContextAccessor _httpContextAccessor;
        //获取Seesion实例
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        //获取Cookie实例
        private IRequestCookieCollection _requestCookies => _httpContextAccessor.HttpContext.Request.Cookies;
        private IResponseCookies _responseCookies => _httpContextAccessor.HttpContext.Response.Cookies;

        //构造函数注入
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


        public void SetCookie(string key, string value, int timespan = 30)
        {
            _responseCookies.Append(key, value, options: new CookieOptions {
                Expires = DateTime.Now.AddMinutes(timespan)
            });
        }

        public string GetCookie(string key)
        {
            string value = string.Empty;
            bool result = _requestCookies.TryGetValue(key, out value);
            return value;
        }
    }
}
