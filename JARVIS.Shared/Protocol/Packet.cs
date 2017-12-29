using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JARVIS.Shared.Protocol
{
    /// <summary>
    /// A JCP Packet (collection of <see cref="T:JARVIS.Shared.Protocol.Instruction"/>)
    /// </summary>
    public class Packet
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:JARVIS.Shared.Protocol.Packet"/> was encryped.
        /// </summary>
        /// <value><c>true</c> if was encryped; otherwise, <c>false</c>.</value>
        public bool WasEncryped { get; private set; }

        /// <summary>
        /// The text encoding used.
        /// </summary>
        public readonly Encoding TextEncoding = Encoding.UTF8;

        /// <summary>
        /// A list of instructions in the packet.
        /// </summary>
        List<Instruction> Instructions = new List<Instruction>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Shared.Protocol.Packet"/> class.
        /// </summary>
        public Packet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Shared.Protocol.Packet"/> class.
        /// </summary>
        /// <param name="operation">An Instruction's Operation.</param>
        /// <param name="parameters">An Instruction's Parameters.</param>
        public Packet(Instruction.OpCode operation, Dictionary<string, string> parameters)
        {
            AddInstruction(operation, parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Shared.Protocol.Packet"/> class.
        /// </summary>
        /// <param name="data">The serialized data of the packet.</param>
        /// <param name="decryptionKey">The decryption key.</param>
        public Packet(byte[] data, string decryptionKey = "max")
        {
            // Determine if we need to decrypt the packet contents
            if (data[0] == JCP.EncryptedMarker)
            {
                // Remove flag
                data = data.Skip(1).ToArray();
                WasEncryped = true;

                // Process it and decrypt
                EncryptionProvider e = new EncryptionProvider(decryptionKey);
                data = e.Decrypt(data);
            }
            else if (data[0] == JCP.DecryptedMarker)
            {
                data = data.Skip(1).ToArray();
            }
            else
            {
                // It's not one of ours leave it the fuck alone, don't even bother to try and figure it out
                return;
            }

            // Reset our instruction list
            Instructions = new List<Instruction>();

            // Create usable working list
            List<byte> workingData = new List<byte>();
            workingData.AddRange(data);

            bool parsing = true;
            while (parsing)
            {

                if (workingData.Count >= JCP.SizeOfLength)
                {
                    byte[] lengthBytes = workingData.GetRange(0, JCP.SizeOfLength).ToArray();
                    int instructionLength = BitConverter.ToInt32(lengthBytes, 0);

                    // We've got a complete packet at this point
                    if (workingData.Count >= (instructionLength + JCP.SizeOfLength))
                    {
                        Instructions.Add(new Instruction(workingData.GetRange(JCP.SizeOfLength, instructionLength).ToArray()));

                        // Remove processed data
                        workingData.RemoveRange(0, JCP.SizeOfLength + instructionLength);
                    }
                    else
                    {
                        parsing = false;
                    }
                }
                else
                {
                    parsing = false;
                }
            }
        }

        /// <summary>
        /// Adds an <see cref="T:JARVIS.Shared.Protocol.Instruction"/> to the packet.
        /// </summary>
        /// <param name="type">The Instruction's operation code.</param>
        /// <param name="parameters">The Instruction's parameters.</param>
        public void AddInstruction(Instruction.OpCode type, Dictionary<string, string> parameters)
        {
            Instructions.Add(new Instruction(type, parameters));
        }

        /// <summary>
        /// A list of <see cref="T:JARVIS.Shared.Protocol.Instruction"/>.
        /// </summary>
        /// <returns>The Instructions</returns>
        public List<Instruction> GetInstructions()
        {
            return Instructions;
        }

        /// <summary>
        /// Get a comma delimited list of Operation Codes
        /// </summary>
        /// <returns>The op codes.</returns>
        public string GetOpCodes()
        {
            string operations = string.Empty;
            foreach (Instruction i in Instructions)
            {
                operations += i.Operation.ToString() + ",";
            }
            operations = operations.TrimEnd(',');
            return operations;
        }

        /// <summary>
        /// Get the bytes of the packet.
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="shouldEncrypt">If set to <c>true</c> the bytes will be encrypted.</param>
        /// <param name="encryptionKey">The encryption key.</param>
        public byte[] ToBytes(bool shouldEncrypt = false, string encryptionKey = "max")
        {
            List<byte> workingBytes = new List<byte>();

            foreach (Instruction i in Instructions)
            {
                byte[] data = i.ToBytes();

                // Add length (4 bytes)
                byte[] lengthData = BitConverter.GetBytes(data.Length);
                workingBytes.AddRange(lengthData);

                // Add data
                workingBytes.AddRange(data);
            }

            if ( shouldEncrypt ) 
            {
                // Process it and decrypt
                EncryptionProvider e = new EncryptionProvider(encryptionKey);
                byte[] returnData = e.Encrypt(workingBytes.ToArray());

                workingBytes.Clear();
                workingBytes.Add(JCP.EncryptedMarker);
                workingBytes.AddRange(returnData);
            }
            else
            {
                workingBytes.Insert(0, JCP.DecryptedMarker);

            }

            return workingBytes.ToArray();
        }
    }
}
