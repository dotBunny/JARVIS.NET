using System;
using System.Security.Cryptography;

namespace JARVIS.Shared.Protocol
{
    public class EncryptionProvider
    {
        public string Key;
        
        public EncryptionProvider(string key)
        {
            Key = key;
        }

        public byte[] Encrypt(byte[] data)
        {
            return Process(data, false);
        }
        public byte[] Decrypt(byte[] data)
        {
            return Process(data, true);
        }

        byte[] Process(byte[] data, bool decrypt = false)
        {
            byte[] returnData = null;

            var key = new Rfc2898DeriveBytes(Key, System.Text.Encoding.ASCII.GetBytes("dotBunny"));

            using (var c = Aes.Create())
            {
                c.BlockSize = 128;
                c.Mode = CipherMode.CBC;
                c.Padding = PaddingMode.ISO10126;
                c.KeySize = 256;
                c.Key = key.GetBytes(32);
                c.IV = key.GetBytes(16);

                if (decrypt)
                {
                    using (var t = c.CreateDecryptor())
                    {
                        returnData = t.TransformFinalBlock(data, 0, data.Length);
                    }
                }
                else
                {
                    using (var t = c.CreateEncryptor())
                    {
                        returnData = t.TransformFinalBlock(data, 0, data.Length);
                    }
                }
            }
            return returnData;
        }
    }
}
