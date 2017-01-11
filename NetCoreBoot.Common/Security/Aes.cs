using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace NetCoreBoot.Common.Security
{
    public class Aes
    {
        /// <summary>
        /// AES加密算法
        /// </summary>
        /// <param name="plainText">待加密字符串</param>
        /// <param name="keys">加密密钥</param>
        /// <returns></returns>
        public static string Encrypt(string plainText, string keys)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");

            if (keys == null || keys.Length < 8)
                throw new ArgumentNullException("keys");
            using (var aesAlg = System.Security.Cryptography.Aes.Create())
            {
                //设置密钥及算法初始化向量
                aesAlg.Key = GeyAesKey(keys);
                aesAlg.IV = GeyAesKey(keys);
                //创建加密器对象
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] encrypted;
                //创建用于加密的流
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt =new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
                StringBuilder ret = new StringBuilder();
                foreach (byte b in encrypted)
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                return ret.ToString();
            }
        }


        public static string Decrypt(string plainText, string keys)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");

            if (keys == null || keys.Length < 8)
                throw new ArgumentNullException("keys");

            int length = plainText.Length / 2;
            byte[] inputBytes = new byte[length];
            int x, i;
            for(x = 0; x < length; x++)
            {
                i = Convert.ToInt32(plainText.Substring(x * 2, 2), 16);
                inputBytes[i] = (byte)i;
            }
            string result;
            using (var aesAlg = System.Security.Cryptography.Aes.Create())
            {
                //设置算法密钥及算法初始化向量
                aesAlg.Key = GeyAesKey(keys);
                aesAlg.IV = GeyAesKey(keys);
                //创建解密器对象
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                //创建用于解密的流
                using (MemoryStream msDecrypt = new MemoryStream(inputBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            result = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取AES密钥 不够32位的密钥补上0,超出32位则截取前32位
        /// </summary>
        /// <param name="key">原始密钥</param>
        /// <returns></returns>
        private static byte[] GeyAesKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "Aes密钥不能为空");
            }
            if(key.Length < 32)
            {
                key = key.PadRight(32, '0');
            }
            if(key.Length > 32)
            {
                key = key.Substring(0, 32);
            }
            return Encoding.UTF8.GetBytes(key);
        }
    }
}
