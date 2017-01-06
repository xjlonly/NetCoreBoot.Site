using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NetCoreBoot.Common
{
    public static class JsonHelper
    {

        public static string Serialize(this object obj)
        {
            var timeConverter = new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        public static string Serialize(this object obj, string datetimeformats)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        public static object Deserialize(this string json)
        {
            return string.IsNullOrEmpty(json) ? new object() : JsonConvert.DeserializeObject(json);
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
