using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public byte Terminator = 0x04;
        public byte EndOfCommand = 0x01;
        public byte EndOfName = 0x02;
        public byte EndOfValue = 0x03;
        public byte EncryptedFalse = 0x0e;
        public byte EncryptedTrue = 0x0f;

        public int Version { get { return ProtocolVersion;  } }
        int ProtocolVersion = 5;
        Encoding ProtocolEncoding = Encoding.UTF8;


        public string EncryptionKey = "max";

        public Protocol(bool encrypt = true, string key = "max")
        {
            UseEncryption = encrypt;
            EncryptionKey = key;
            Log.Message("Socket", "Using Protocol Version " + Version.ToString());
        }

        public Packet GetPacket(byte[] packet)
        {
            Packet returnPacket = new Packet();

            // Handle Decryption
            // We forcibly check for the encryption flag, not relying on settings
            if (packet[0] == EncryptedTrue)
            {
                // Remove flag
                packet = packet.Skip(1).ToArray();

                // create a key from the password and salt, use 32K iterations
                var key = new Rfc2898DeriveBytes(EncryptionKey,
                                                 ProtocolEncoding.GetBytes("dotBunny"));

                using (Aes aes = new AesManaged())
                {
                    // set the key size to 256
                    aes.KeySize = 256;
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);
                    aes.Padding = PaddingMode.ISO10126;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(packet, 0, packet.Length);
                            cs.Close();
                        }
                        packet = ms.ToArray();
                    }
                }
            }
            else if (packet[0] == EncryptedFalse)
            {
                packet = packet.Skip(1).ToArray();
            }

            // Create usable working list
            List<byte> workingPacket = new List<byte>();
            workingPacket.AddRange(packet);

            // Identify command
            int commandEndIndex = workingPacket.IndexOf(EndOfCommand);
            returnPacket.Command = Commands.GetType(ProtocolEncoding.GetString(workingPacket.GetRange(0, commandEndIndex).ToArray()));
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
                    ProtocolEncoding.GetString(chunk.GetRange(0, endOfName).ToArray()).Trim(),
                    ProtocolEncoding.GetString(chunk.GetRange(endOfName+1, chunk.Count - (endOfName+1)).ToArray()).Trim());

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
                    returnParams.Add(parameters[i], parameters[i + 1]);
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
                byteBuilder.AddRange(ProtocolEncoding.GetBytes(s.Trim()));
                byteBuilder.Add(EndOfName);
                byteBuilder.AddRange(ProtocolEncoding.GetBytes(parameters[s].Trim()));
                byteBuilder.Add(EndOfValue);
            }
            return byteBuilder.ToArray();
        }


        public byte[] GetBytes(Commands.Types type, Dictionary<string, string> parameters)
        {
            List<byte> byteBuilder = new List<byte>();

            // Add command to bytes
            byteBuilder.AddRange(ProtocolEncoding.GetBytes(type.ToString()));

            // Add end of command mark
            byteBuilder.Add(EndOfCommand);

            // Add parameters
            byteBuilder.AddRange(GetParameterBytes(parameters));

            // Add end of transmission
            byteBuilder.Add(Terminator);

            // Not encrypting, send it backs
            if (!UseEncryption ) {
                byteBuilder.Insert(0, EncryptedFalse);
                return byteBuilder.ToArray();
            }

            // Looks like we are encrypting so lets do this!
            List<byte> encryptedBytes = new List<byte>();

            // create a key from the password and salt, use 32K iterations – see note
            var key = new Rfc2898DeriveBytes(EncryptionKey,
                                             ProtocolEncoding.GetBytes("dotBunny"));

            // create an AES object
            using (Aes aes = new AesManaged())
            {
                // set the key size to 256
                aes.KeySize = 256;
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);
                aes.Padding = PaddingMode.ISO10126;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(byteBuilder.ToArray(), 0, byteBuilder.Count);
                        cs.Close();
                    }
                    encryptedBytes.AddRange(ms.ToArray());
                }
            }

            // Add flag that it is encrypted data
            encryptedBytes.Insert(0, EncryptedTrue);

            return encryptedBytes.ToArray();
        }

    }
}
