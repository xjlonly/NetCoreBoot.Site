using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace NetCoreBoot.Common
{
    public class Des
    {
        IBlockCipher engine = new DesEngine();
        public string Encrypt(string plainText, string keys)
        {

            byte[] ptBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] rv = Encrypt(keys, ptBytes);
            StringBuilder ret = new StringBuilder();
            foreach (byte b in rv)
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        private byte[] Encrypt(string keys, byte[] ptBytes)
        {
            //byte[] key = Encoding.UTF8.GetBytes(keys);
            byte[] key = Encoding.UTF8.GetBytes(Hash.MD5(keys).Substring(0, 8));
            BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(engine), new Pkcs7Padding());
            cipher.Init(true, new ParametersWithIV(new DesParameters(key), key));
            byte[] rv = new byte[cipher.GetOutputSize(ptBytes.Length)];
            int tam = cipher.ProcessBytes(ptBytes, 0, ptBytes.Length, rv, 0);

            cipher.DoFinal(rv, tam);
            return rv;
        }

    }
}
