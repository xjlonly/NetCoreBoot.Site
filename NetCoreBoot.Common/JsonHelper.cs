using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NetCoreBoot.Common
{
    public static class JsonHelper
    {
        public static string Serialize(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(this string json)
        {
            return string.IsNullOrEmpty(json) ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }

        public static List<T> DeserializeToList<T>(this string json)
        {
            return string.IsNullOrEmpty(json) ? default(List<T>) : JsonConvert.DeserializeObject<List<T>>(json);
        }
    }
}
