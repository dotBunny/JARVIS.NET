using System;
using System.Collections.Generic;
using System.Text;

namespace JARVIS.Shared.Protocol
{
    /// <summary>
    /// JCP Instruction
    /// </summary>
    public class Instruction
    {
        /// <summary>
        /// Protocol Operation Codes
        /// </summary>
        public enum OpCode
        {
            DEFAULT,
            PING,
            PONG,
            AUTH,
            LOGIN,
            FAIL,
            INFO,
            COUNTER_SET,
            COUNTER_PLUS,
            COUNTER_MINUS,
            WIRECAST_LAYERS
        }

        /// <summary>
        /// Instruction Operation Code
        /// </summary>
        public OpCode Operation = OpCode.DEFAULT;

        /// <summary>
        /// Instruction Parameters
        /// </summary>
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Shared.Protocol.Instruction"/> class.
        /// </summary>
        public Instruction()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Shared.Protocol.Instruction"/> class.
        /// </summary>
        /// <param name="operation">The Instruction's Operation.</param>
        /// <param name="parameters">The Instruction's Parameters.</param>
        public Instruction(OpCode operation, Dictionary<string, string> parameters)
        {
            Operation = operation;
            Parameters = parameters;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Shared.Protocol.Instruction"/> class with the provided data.
        /// </summary>
        /// <param name="data">Byte data array used to represent class in a serialized form.</param>
        public Instruction(byte[] data)
        {
            // Create usable working list
            List<byte> workingPacket = new List<byte>();
            workingPacket.AddRange(data);

            // Identify command
            int commandEndIndex = workingPacket.IndexOf(JCP.OpCodeTerminator);
            Operation = GetOpCode(Encoding.UTF8.GetString(workingPacket.GetRange(0, commandEndIndex).ToArray()));
            workingPacket.RemoveRange(0, commandEndIndex + 1);

            // Breakdown remaining parts of the packet
            bool parsing = true;
            while (parsing)
            {
                int findNextChunkEnd = workingPacket.IndexOf(JCP.ParameterValueTerminator, 0);

                if (findNextChunkEnd < 0)
                {
                    parsing = false;
                    continue;
                }

                List<byte> chunk = workingPacket.GetRange(0, findNextChunkEnd);

                // Split chunk
                int endOfName = chunk.IndexOf(JCP.ParameterNameTerminator);

                Parameters.Add(
                    Encoding.UTF8.GetString(chunk.GetRange(0, endOfName).ToArray()).Trim(),
                    Encoding.UTF8.GetString(chunk.GetRange(endOfName + 1, chunk.Count - (endOfName + 1)).ToArray()).Trim());

                workingPacket.RemoveRange(0, findNextChunkEnd + 1);
            }
        }

        /// <summary>
        /// Gets the enumerated operations code from the provided string.
        /// </summary>
        /// <returns>The OpCode.</returns>
        /// <param name="type">A string value representing the operations code.</param>
        public static OpCode GetOpCode(string type)
        {
            // Sanitize Type
            type = type.Replace(".", "_").Replace("/", "_");

            OpCode returnType = OpCode.DEFAULT;
            if (Enum.TryParse(type, true, out returnType))
            {
                return returnType;
            }
            return OpCode.DEFAULT;
        }

        /// <summary>
        /// Converts the instruction to a byte array.
        /// </summary>
        /// <returns>The serialzied bytes.</returns>
        public byte[] ToBytes()
        {
            List<byte> byteBuilder = new List<byte>();

            // Add command to bytes
            byteBuilder.AddRange(Encoding.UTF8.GetBytes(Operation.ToString()));

            // Add end of command mark
            byteBuilder.Add(JCP.OpCodeTerminator);

            // Handle Parameters
            foreach (string s in Parameters.Keys)
            {
                // Add name
                byteBuilder.AddRange(Encoding.UTF8.GetBytes(s.Trim()));
                byteBuilder.Add(JCP.ParameterNameTerminator);
                byteBuilder.AddRange(Encoding.UTF8.GetBytes(Parameters[s].Trim()));
                byteBuilder.Add(JCP.ParameterValueTerminator);
            }

            return byteBuilder.ToArray();
        }
    }
}
