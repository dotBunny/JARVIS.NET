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

        // Byte Codes
        public static byte[] Terminator = { 0x04 };
        public static byte EndOfCommand = 0x01;
        public static byte EndOfName = 0x02;
        public static byte EndOfValue = 0x03;

        public static string EncryptionKey = "max";

        public Protocol(string key = "max")
        {
            EncryptionKey = key;
        }

        public Packet GetPacket(byte[] packet)
        {
            Packet returnPacket = new Packet();
#if !DEBUG  
            packet = Decrypt(packet);
#endif

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
       
        public static Dictionary<string, string> GetStringDictionary(string[] parameters)
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
        public static byte[] GetParameterBytes(Dictionary<string, string> parameters)
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
        public static byte[] GetBytes(Commands.Types type, Dictionary<string, string> parameters)
        {
            List<byte> byteBuilder = new List<byte>();

            // Add command to bytes
            byteBuilder.AddRange(Encoding.ASCII.GetBytes(type.ToString()));

            // Add end of command mark
            byteBuilder.Add(EndOfCommand);

            // Add parameters
            byteBuilder.AddRange(GetParameterBytes(parameters));

#if !DEBUG
            byte[] encrypted = Encrypt(byteBuilder.ToArray());
            byteBuilder.Clear();
            byteBuilder.AddRange(encrypted);
#endif
            // Add end of transmission
            byteBuilder.AddRange(Terminator);

            return byteBuilder.ToArray();
        }





        private static SymmetricAlgorithm GetAlgorithm()
        {
            var algorithm = Rijndael.Create();
            var rdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x53,0x6f,0x62,0x69,0x75,0x6d,0x20,0x43,0x68,0x6c,0x6f,0x71,0x69,0x64,0x65 });
            algorithm.Padding = PaddingMode.ISO10126;
            algorithm.Key = rdb.GetBytes(32);
            algorithm.IV = rdb.GetBytes(16);
            return algorithm;
        }


        private static byte[] Encrypt(byte[] data)
        {
            var algorithm = GetAlgorithm();
            var encryptor = algorithm.CreateEncryptor();
            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.Close();
                return ms.ToArray();
            }
        }

        private static byte[] Decrypt(byte[] data)
        {
            var algorithm = GetAlgorithm();
            var decryptor = algorithm.CreateDecryptor();

            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.Close();
                return ms.ToArray();

            }
        }
    }
}
