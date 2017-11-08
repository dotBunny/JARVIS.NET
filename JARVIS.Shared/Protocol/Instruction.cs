using System;
using System.Collections.Generic;
using System.Text;

namespace JARVIS.Shared.Protocol
{
    public class Instruction
    {
        public enum OpCode
        {
            DEFAULT,
            PING,
            PONG,
            AUTH,
            LOGIN,
            INFO,
            COUNTER_SET,
            COUNTER_PLUS,
            COUNTER_MINUS,
            WIRECAST_LAYERS
        }

        public OpCode Operation = OpCode.DEFAULT;
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();

        public Instruction()
        {
            
        }

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

        public Instruction(OpCode operation, Dictionary<string, string> parameters) {
            Operation = operation;
            Parameters = parameters;
        }

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
