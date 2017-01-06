using System;
using Http = Microsoft.AspNetCore.Http;

namespace NetCoreBoot.Common
{
    public static class HttpContext
    {
        public static IServiceProvider ServiceProvider;
        static HttpContext()
        {

        }

        public static Http.HttpContext Context
        {
            get
            {
                object factory = ServiceProvider.GetService(typeof(Http.IHttpContextAccessor));
                Http.HttpContext context = ((Http.IHttpContextAccessor)factory).HttpContext;
                return context;
            }
        }
    }
}
