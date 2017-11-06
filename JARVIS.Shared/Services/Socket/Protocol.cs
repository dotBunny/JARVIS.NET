using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JARVIS.Shared.Services.Socket
{
    /// <summary>
    /// Custom 'Secure' JARVIS Protocol
    /// </summary>
    public class Protocol
    {
        public class Packet
        {
            public Commands.Types Command;
            public Dictionary<string, string> Parameters = new Dictionary<string, string>();
        }

        public bool UseEncryption = true;

        // Byte Codes
        public byte[] Terminator = { 0x04 };
        public byte EndOfCommand = 0x01;
        public byte EndOfName = 0x02;
        public byte EndOfValue = 0x03;



        public string EncryptionKey = "max";

        public Protocol(bool encrypt = true, string key = "max")
        {
            UseEncryption = encrypt;
            EncryptionKey = key;
        }

        public Packet GetPacket(byte[] packet)
        {
            Packet returnPacket = new Packet();

            // Drop terminator byte
            if ( packet[packet.Length - 1] == 0x04 ) {
                Array.Resize(ref packet, packet.Length - 1);
            }
            packet = Decrypt(packet);

            // Create usable working list
            List<byte> workingPacket = new List<byte>();
            workingPacket.AddRange(packet);

            // Identify command
            int commandEndIndex = workingPacket.IndexOf(EndOfCommand);
            returnPacket.Command = Commands.GetType(Encoding.ASCII.GetString(workingPacket.GetRange(0, commandEndIndex).ToArray()));
            workingPacket.RemoveRange(0, commandEndIndex + 1);

            // Breakdown remaining parts of the packet
            bool parsing = true;
            while (parsing)
            {
                int findNextChunkEnd = workingPacket.IndexOf(EndOfValue, 0);

                if (findNextChunkEnd < 0)
                {
                    parsing = false;
                    continue;
                }

                List<byte> chunk = workingPacket.GetRange(0, findNextChunkEnd);

                // Split chunk
                int endOfName = chunk.IndexOf(EndOfName);

                returnPacket.Parameters.Add(
                    Encoding.ASCII.GetString(chunk.GetRange(0, endOfName).ToArray()), 
                    Encoding.ASCII.GetString(chunk.GetRange(endOfName, chunk.Count - endOfName).ToArray()));

                workingPacket.RemoveRange(0, findNextChunkEnd + 1);
            }

            return returnPacket;
        }
       
        public Dictionary<string, string> GetStringDictionary(string[] parameters)
        {
            // Split out parameters
            // i should split them every other here?
            Dictionary<string, string> returnParams = new Dictionary<string, string>();

            for (int i = 0; i < parameters.Length; i+=2)
            {
                
                if (i+1 < parameters.Length)
                {
                    returnParams.Add(parameters[i].Trim(), parameters[i + 1].Trim());
                }
            }
            return returnParams;
        }

        // TODO Add encryption to ascii
        public byte[] GetParameterBytes(Dictionary<string, string> parameters)
        {
            List<byte> byteBuilder = new List<byte>();
            foreach (string s in parameters.Keys)
            {
                // Add name
                byteBuilder.AddRange(Encoding.ASCII.GetBytes(s.Trim()));
                byteBuilder.Add(EndOfName);
                byteBuilder.AddRange(Encoding.ASCII.GetBytes(parameters[s].Trim()));
                byteBuilder.Add(EndOfValue);
            }
            return byteBuilder.ToArray();
        }

        // TODO: Add encryption on the string before getting the bytes
        public byte[] GetBytes(Commands.Types type, Dictionary<string, string> parameters)
        {
            List<byte> byteBuilder = new List<byte>();

            // Add command to bytes
            byteBuilder.AddRange(Encoding.ASCII.GetBytes(type.ToString()));

            // Add end of command mark
            byteBuilder.Add(EndOfCommand);

            // Add parameters
            byteBuilder.AddRange(GetParameterBytes(parameters));

            byte[] encrypted = Encrypt(byteBuilder.ToArray());
            byteBuilder.Clear();
            byteBuilder.AddRange(encrypted);

            // Add end of transmission
            byteBuilder.AddRange(Terminator);

            return byteBuilder.ToArray();
        }




 
        public byte[] Encrypt(byte[] data)
        {
            if (!UseEncryption) return data;
            byte[] encryptedBytes = null;

            // create a key from the password and salt, use 32K iterations – see note
            var key = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x53, 0x6f, 0x62, 0x69, 0x75, 0x6d, 0x20, 0x43, 0x68, 0x6c, 0x6f, 0x71, 0x69, 0x64, 0x65 });

            // create an AES object
            using (Aes aes = new AesManaged())
            {
                // set the key size to 256
                aes.KeySize = 256;
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }
            return encryptedBytes;
        }

        private byte[] Decrypt(byte[] cryptBytes)
        {
            if (!UseEncryption) return cryptBytes;
            byte[] clearBytes = null;

            // create a key from the password and salt, use 32K iterations
            var key = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x53, 0x6f, 0x62, 0x69, 0x75, 0x6d, 0x20, 0x43, 0x68, 0x6c, 0x6f, 0x71, 0x69, 0x64, 0x65 });

            using (Aes aes = new AesManaged())
            {
                // set the key size to 256
                aes.KeySize = 256;
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cryptBytes, 0, cryptBytes.Length);
                        cs.Close();
                    }
                    clearBytes = ms.ToArray();
                }
            }
            return clearBytes;
        }



    }
}
