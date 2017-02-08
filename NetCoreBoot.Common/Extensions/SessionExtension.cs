using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace NetCoreBoot.Common
{
    public static class SessionExtension
    {
        //读取Session存储的对象数据
        public static T Get<T>(this ISession session, string key) where T : class
        {
            byte[] byteArray = null;
            if(session.TryGetValue(key, out byteArray))
            { 
                return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(byteArray));
            }
            return null;
        }

        public static void Set<T>(this ISession session, string key, T value) where T : class
        {
            try
            {
                var str = JsonConvert.SerializeObject(value);
                byte[] byteArray = Encoding.UTF8.GetBytes(str);
                session.Set(key, byteArray);
            }
            catch(Exception ex)
            {
                throw new Exception($"序列化失败。错误信息：{ex}");
            }
        }
    }
}
