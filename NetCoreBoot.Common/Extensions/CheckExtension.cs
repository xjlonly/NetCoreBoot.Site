using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBoot.Common
{
    public static class CheckExtension
    {
        //判断对象是否为空
        public static bool NotNull<T>(this T str)
        {
            return str == null ? false : true; 
        }
        public static void NotNull<T>(this T str, string errorMsg = null)
        {
            if(str == null)
            {
                throw new InvalidDataException(errorMsg);
            }
        }

        public static void NotNullOrEmpty(this object str, string errorMsg = null)
        {
            if (str is string)
            {
                NotNullOrEmpty((string)str, errorMsg);
                return;
            }
            else
            {
                NotNull(str, errorMsg);
            }
        }

        public static void NotNullOrEmpty(this string str, string errorMsg = null)
        {
            if (string.IsNullOrEmpty(str))
                throw new InvalidDataException(errorMsg);
        }
        //判断是否非空
        public static bool NotNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str) ? false : true;
        }
        /// <summary>
        /// 确保对象是否有效
        /// </summary>
        /// <param name="obj"></param>
        public static void EnsureValid(this object obj)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext vc = new ValidationContext(obj, null, null);
            bool isValid = Validator.TryValidateObject
                    (obj, vc, validationResults, true);
            if (isValid == false)
            {
                throw new InvalidDataException(validationResults[0].ErrorMessage);
            }
        }


        #region 删除最后一个字符之后的字符
        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(this string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(this string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }
        /// <summary>
        /// 删除最后结尾的长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string DelLastLength(this string str, int Length)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            str = str.Substring(0, str.Length - Length);
            return str;
        }
        #endregion
    }
}
