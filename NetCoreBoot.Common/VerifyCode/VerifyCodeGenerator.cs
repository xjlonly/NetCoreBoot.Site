using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBoot.Common
{
    internal class VerifyCodeGenerator
    {
        internal static string GenVerifyCode()
        {
            string ckCode = string.Empty;
            char[] characters = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            Random rdm = new Random();
            //生成验证码字符串
            for(int i = 0; i < 4; i ++)
            {
                ckCode += characters[rdm.Next(characters.Length)];
            }
            return ckCode;
        }
    }
}
