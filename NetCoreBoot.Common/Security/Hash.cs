using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBoot.Common
{
    public class Hash
    {
        /// <summary>
        /// MD5加密算法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5(string input, Encoding encode = null)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                encode = encode.NotNull() ? encode : Encoding.UTF8;
                var result = md5.ComputeHash(encode.GetBytes(input));
                return BitConverter.ToString(result);
            }
           
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string SHA1(string input, Encoding encode = null)
        {
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                encode = encode.NotNull() ? encode : Encoding.UTF8;
                var result = sha1.ComputeHash(encode.GetBytes(input));
                return BitConverter.ToString(result);
            }
        }
    }
}
