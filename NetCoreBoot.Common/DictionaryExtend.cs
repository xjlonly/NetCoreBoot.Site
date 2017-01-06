using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBoot.Common
{
    public static class DictionaryExtend
    {

        /// <summary>
        /// 字典信息填充流
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="stream"></param>
        public static void FillFormDataStream(this Dictionary<string, string> formData, Stream stream)
        {
            string dataString = formData.GetQueryString();
            var bytes = string.IsNullOrEmpty(dataString) ? new byte[0] : Encoding.UTF8.GetBytes(dataString);
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// 字段组装查询参数
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        private static string GetQueryString(this Dictionary<string, string> formData)
        {
            if (formData == null || formData.Count == 0)
            {
                return string.Empty;
            }
            StringBuilder sbStr = new StringBuilder();
            int index = 0;
            foreach (var kv in formData)
            {
                index++;
                sbStr.AppendFormat("{0}={1}", kv.Key, kv.Value);
                if (index < formData.Count)
                {
                    sbStr.Append("&");
                }

            }

            return sbStr.ToString();
        }
    }
}
