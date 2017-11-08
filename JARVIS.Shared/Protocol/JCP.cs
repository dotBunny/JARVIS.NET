using System;
using System.Collections.Generic;

namespace JARVIS.Shared.Protocol
{
    /// <summary>
    /// Custom 'Secure' JARVIS Protocol
    /// </summary>
    public class JCP
    {
        public const byte LengthTerminator = 0x01;
        public const byte OpCodeTerminator = 0x02;
        public const byte ParameterNameTerminator = 0x03;
        public const byte ParameterValueTerminator = 0x04;
        public const byte DecryptedMarker = 0x0e;
        public const byte EncryptedMarker = 0x0f;
        public const byte TransmissionTerminator = 0x7f;

        public const int Version = 10;

        public string EncryptionKey = "max";
        public bool UseEncryption = true;

        public JCP(bool useEncryption = true, string encryptionKey = "max")
        {
            UseEncryption = useEncryption;
            EncryptionKey = encryptionKey;

            Log.Message("Socket", "Using JCP Version " + Version.ToString());
        }

        /// <summary>
        /// Get packets from data buffer provided. 
        /// This seems like a double up, but what it does is actually protect against buffer joins when reading from the socket.
        /// </summary>
        /// <returns>The packets from data.</returns>
        /// <param name="data">A byte[] array of data to parse.</param>
        public Packet[] GetPackets(byte[] data)
        {
            List<Packet> returnPackets = new List<Packet>();

            // Create usable working list
            List<byte> workingData = new List<byte>();
            workingData.AddRange(data);

            bool parsing = true;
            while (parsing)
            {
                int findNextEndOfLength = workingData.IndexOf(LengthTerminator, 0);
                if (findNextEndOfLength < 0)
                {
                    parsing = false;
                    continue;
                }

                // Get the length data that we will need to read
                byte[] lengthData = workingData.GetRange(0, findNextEndOfLength).ToArray();

                int length = BitConverter.ToInt32(lengthData, 0);
                byte[] packetData = workingData.GetRange(findNextEndOfLength + 1, length).ToArray();

                // Create a new packet with the data provided
                returnPackets.Add(new Packet(packetData, EncryptionKey));

                // Remove the data we just used
                workingData.RemoveRange(0, findNextEndOfLength + 1 + length);
            }

            return returnPackets.ToArray();
        }

        public byte[] GetBytes(Packet packet)
        {
            Packet[] quickArray =  { packet };
            return GetBytes(quickArray);
        }

        public byte[] GetBytes(Packet[] packets)
        {
            List<byte> returnBytes = new List<byte>();

            foreach(Packet p in packets)
            {
                byte[] data = p.ToBytes(UseEncryption, EncryptionKey);

                // Add length
                byte[] lengthData = BitConverter.GetBytes(data.Length);
                returnBytes.AddRange(lengthData);

                // Add end of length
                returnBytes.Add(LengthTerminator);

                // Add data
                returnBytes.AddRange(data);
            }

            // Add termination that we can externally check to know when to process
            returnBytes.Add(TransmissionTerminator);

            return returnBytes.ToArray();
        }


 



        //public Dictionary<string, string> GetStringDictionary(string[] parameters)
        //{
        //    // Split out parameters
        //    // i should split them every other here?
        //    Dictionary<string, string> returnParams = new Dictionary<string, string>();

        //    for (int i = 0; i < parameters.Length; i+=2)
        //    {
        //        if (i+1 < parameters.Length)
        //        {
        //            returnParams.Add(parameters[i], parameters[i + 1]);
        //        }
        //    }
        //    return returnParams;
        //}

     
      
    }

}
