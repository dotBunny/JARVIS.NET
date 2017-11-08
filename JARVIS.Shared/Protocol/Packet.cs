using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JARVIS.Shared.Protocol
{
    public class Packet
    {
        public readonly Encoding TextEncoding = Encoding.UTF8;

        public bool WasEncryped = false;

        List<Instruction> Instructions = new List<Instruction>();
        public List<Instruction> GetInstructions()
        {
            return Instructions;
        }
        public Packet()
        {
            
            
        }

        public string GetOpCodes()
        {
            string operations = string.Empty;
            foreach(Instruction i in Instructions)
            {
                operations += i.Operation.ToString() + ",";
            }
            operations = operations.TrimEnd(',');
            return operations;
        }

        public Packet(Instruction.OpCode operation, Dictionary<string, string> parameters)
        {
            AddInstruction(operation, parameters);
        }

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
                int findNextEndOfLength = workingData.IndexOf(JCP.LengthTerminator, 0);
                if (findNextEndOfLength < 0)
                {
                    parsing = false;
                    continue;
                }

                // Get the length data that we will need to read
                byte[] lengthData = workingData.GetRange(0, findNextEndOfLength).ToArray();

                int length = BitConverter.ToInt32(lengthData, 0);
                byte[] packetData = workingData.GetRange(findNextEndOfLength + 1, length).ToArray();

                // Create a new instruction with the data provided
                Instructions.Add(new Instruction(packetData));

                // Remove the data we just used
                workingData.RemoveRange(0, findNextEndOfLength + 1 + length);
            }
        }


        public void AddInstruction(Instruction.OpCode type, Dictionary<string, string> parameters)
        {
            Instructions.Add(new Instruction(type, parameters));
        }


        public byte[] ToBytes(bool shouldEncrypt = false, string encryptionKey = "max")
        {
            List<byte> workingBytes = new List<byte>();

            foreach (Instruction i in Instructions)
            {
                byte[] data = i.ToBytes();

                // Add length
                byte[] lengthData = BitConverter.GetBytes(data.Length);
                workingBytes.AddRange(lengthData);

                // Add end of length
                workingBytes.Add(JCP.LengthTerminator);

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
