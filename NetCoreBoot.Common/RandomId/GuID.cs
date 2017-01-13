using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBoot.Common
{
    public class GuID
    {
        /// <summary>
        /// 生成全局唯一标识符(GUID)
        /// </summary>
        /// <returns></returns>
        public static string CreateGuid()
        {
            return System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 自动生成编号
        /// </summary>
        /// <returns></returns>
        public static string CreateNo()
        {
            Random rand = new Random();
            string rText = rand.Next(1000, 100000).ToString();
            string code = DateTime.Now.ToString("yyyyMMddHHmmss") + rText.PadLeft(5, '0');
            return code;
        }
    }
}
