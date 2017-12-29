using System;
using System.Collections.Generic;

namespace JARVIS.Shared.Protocol
{
    /// <summary>
    /// Custom 'Secure' JARVIS Protocol
    /// </summary>
    public class JCP
    {
        /// <summary>
        /// An indicator byte that data is not encrypted.
        /// </summary>
        public const byte DecryptedMarker = 0x0e;

        /// <summary>
        /// An indicator byte that data is encrypted.
        /// </summary>
        public const byte EncryptedMarker = 0x0f;

        /// <summary>
        /// The indicator byte of the data length definition.
        /// </summary>
        public const byte LengthTerminator = 0x01;

        /// <summary>
        /// The indicator byte of the operation code.
        /// </summary>
        public const byte OpCodeTerminator = 0x02;

        /// <summary>
        /// The indicator byte of a parameter name.
        /// </summary>
        public const byte ParameterNameTerminator = 0x03;

        /// <summary>
        /// The indicator byte of a parameter value.
        /// </summary>
        public const byte ParameterValueTerminator = 0x04;

        /// <summary>
        /// The indicator byte of the end of the transmission.
        /// </summary>
        public const byte TransmissionTerminator = 0x7f;

        /// <summary>
        /// Protocol Version
        /// </summary>
        public const int Version = 12;

        /// <summary>
        /// The Encryption Key
        /// </summary>
        public string EncryptionKey
        {
            get
            {
                if (IsAuthenticated)
                {
                    return currentEncryptionKey;
                }
                return initialEncryptionKey;
            }
        }

        /// <summary>
        /// Has the JCP protocol session been authenticated? 
        /// </summary>
        public bool IsAuthenticated = false;

        /// <summary>
        /// Should encryption be used?
        /// </summary>
        public bool UseEncryption = true;

        /// <summary>
        /// The current encryption key
        /// </summary>
        string currentEncryptionKey;

        /// <summary>
        /// The encryption key used during the login process.
        /// </summary>
        string initialEncryptionKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Shared.Protocol.JCP"/> class.
        /// </summary>
        /// <param name="useEncryption">If set to <c>true</c> packets should be encrypted.</param>
        /// <param name="encryptionKey">The encryption key.</param>
        public JCP(bool useEncryption = true, string encryptionKey = "max")
        {
            UseEncryption = useEncryption;
            initialEncryptionKey = encryptionKey;
            currentEncryptionKey = encryptionKey;

            Log.Message("Socket", "Using JCP Version " + Version.ToString());
        }

        /// <summary>
        /// Gets the bytes of a given packet.
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="packet">The target packet.</param>
        public byte[] GetBytes(Packet packet)
        {
            Packet[] quickArray = { packet };
            return GetBytes(quickArray);
        }
        /// <summary>
        /// Gets the bytes of an array of packets.
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="packets">The packets.</param>
        public byte[] GetBytes(Packet[] packets)
        {
            List<byte> returnBytes = new List<byte>();

            foreach (Packet p in packets)
            {
                byte[] data = p.ToBytes(UseEncryption, EncryptionKey);

                if (data.Length > 0)
                {
                    // Add length
                    byte[] lengthData = BitConverter.GetBytes(data.Length);
                    returnBytes.AddRange(lengthData);

                    // Add end of length
//                    returnBytes.Add(LengthTerminator);

                    // Add data
                    returnBytes.AddRange(data);
                }
            }

            // Add termination that we can externally check to know when to process
         //   returnBytes.Add(TransmissionTerminator);

            return returnBytes.ToArray();
        }
    }

}
