using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Authentication;
using NetCoreBoot.Common;

namespace NetCoreBoot.CommonApplication
{
    public class AdminUtils
    {
        public const string STokenName = "stoken";
        public static WebHelper webHelper = new WebHelper(HttpContext.Accessor);
        public static LoginCookie GetCurrentCookie()
        {
            var userData =  webHelper.GetCookie(STokenName);
            try
            {
                LoginCookie cookie = JsonHelper.Deserialize<LoginCookie>(userData);
                return cookie;
            }
            catch
            {
                return null;
            }
        }
        public static void SetLoginCookie(LoginCookie cookie)
        {
            if (cookie != null)
            {
                string encryptedTicket =  JsonHelper.Serialize(cookie);
                webHelper.SetCookie(STokenName, encryptedTicket, 60);
            }
        }
        public static void AbandonCookie()
        {
            webHelper.SetCookie(STokenName, "", -1);
        }
    }
}
