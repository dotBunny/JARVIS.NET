using System;
using System.Security.Cryptography;
using System.Text;

namespace JARVIS.Shared.Protocol
{
    public class EncryptionProvider
    {
        /// <summary>
        /// The encryption key to use.
        /// </summary>
        string encryptionKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Shared.Protocol.EncryptionProvider"/> class with the provided encryption key.
        /// </summary>
        /// <param name="key">The encryption key.</param>
        public EncryptionProvider(string key)
        {
            encryptionKey = key;
        }

        /// <summary>
        /// Decrypt the specified data.
        /// </summary>
        /// <returns>The decrypted bytes.</returns>
        /// <param name="data">Encrypted bytes.</param>
        public byte[] Decrypt(byte[] data)
        {
            return Process(data, true);
        }

        /// <summary>
        /// Encrypt the specified data.
        /// </summary>
        /// <returns>The encrypted bytes.</returns>
        /// <param name="data">Decrypted bytes.</param>
        public byte[] Encrypt(byte[] data)
        {
            return Process(data, false);
        }

        /// <summary>
        /// Process the specified data
        /// </summary>
        /// <returns>The processed data</returns>
        /// <param name="data">The data to manipulate.</param>
        /// <param name="decrypt">If set to <c>true</c> decrypt the provided data, otherwise encrypt.</param>
        byte[] Process(byte[] data, bool decrypt = false)
        {
            byte[] returnData = null;

            var key = new Rfc2898DeriveBytes(encryptionKey, System.Text.Encoding.ASCII.GetBytes("dotBunny"));

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
