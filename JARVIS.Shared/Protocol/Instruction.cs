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
            /// <summary>
            /// Non-event based protocol operation that should do nothing.
            /// </summary>
            DEFAULT,

            /// <summary>
            /// The originating sacred 'PING' command which will force a 'PONG' response.
            /// </summary>
            PING,

            /// <summary>
            /// The response to a 'PING'.
            /// </summary>
            PONG,

            /// <summary>
            /// A request for authentication, when sent from server, it will force clients to send 'LOGIN' information back. 
            /// When sent from a client, it will force the server to send it back to them to reauth.
            /// </summary>
            AUTH,

            /// <summary>
            /// Login credentials (sent from client to server).
            /// </summary>
            LOGIN,

            /// <summary>
            /// Sent to clients when login has failed.
            /// </summary>
            LOGIN_FAIL,

            /// <summary>
            /// Informational packet sent for debugging information.
            /// </summary>
            INFO,

            /// <summary>
            /// Adjust Wirecast layers on a client.
            /// </summary>
            WIRECAST_LAYERS,

            /// <summary>
            /// A request for clients/shards to authenticate a service via OAuth.
            /// </summary>
            OAUTH_REQUEST,

            /// <summary>
            /// Force server (only handler) to reauthenticate with Discord.
            /// </summary>
            AUTH_DISCORD,

            /// <summary>
            /// Force server (only handler) to reauthenticate with Spotify.
            /// </summary>
            AUTH_SPOTIFY,

            /// <summary>
            /// Force server (only handler) to reauthenticate with Streamlabs.
            /// </summary>
            AUTH_STREAMLABS,

            /// <summary>
            /// Send text file to remote system (filename, content).
            /// </summary>
            TEXT_FILE,

            /// <summary>
            /// Send binary file to remote system (filename, content).
            /// </summary>
            BINARY_FILE 
        }

        /// <summary>
        /// The instructions operation code.
        /// </summary>
        public OpCode Operation = OpCode.DEFAULT;

        /// <summary>
        /// The instructions parameters to pass to the operation's command.
        /// </summary>
        public Dictionary<string, InstructionParameter> Parameters = new Dictionary<string, InstructionParameter>();

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
        public Instruction(OpCode operation, Dictionary<string, InstructionParameter> parameters)
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

            byte[] operationLengthData = workingPacket.GetRange(0, Platform.ByteSizeOfInt).ToArray();
            int operationLength = BitConverter.ToInt32(operationLengthData, 0);

            // Get our operation code
            Operation = GetOpCode(Encoding.UTF8.GetString(workingPacket.GetRange(Platform.ByteSizeOfInt, operationLength).ToArray()));

            // Remove processed operation data
            workingPacket.RemoveRange(0, Platform.ByteSizeOfInt + operationLength);

            bool parsing = true;
            while(parsing)
            {
                if ( workingPacket.Count > Platform.ByteSizeOfInt)
                {

                    byte[] parameterKeyLengthData = workingPacket.GetRange(0, Platform.ByteSizeOfInt).ToArray();
                    int parameterKeyLength = BitConverter.ToInt32(parameterKeyLengthData, 0);
                    byte[] parameterKeyData = workingPacket.GetRange(Platform.ByteSizeOfInt, parameterKeyLength).ToArray();
                    workingPacket.RemoveRange(0, Platform.ByteSizeOfInt + parameterKeyLength);

                    byte[] parameterValueLengthData = workingPacket.GetRange(0, Platform.ByteSizeOfInt).ToArray();
                    int parameterValueLength = BitConverter.ToInt32(parameterValueLengthData, 0);
                    byte[] parameterValueData = workingPacket.GetRange(Platform.ByteSizeOfInt, parameterValueLength).ToArray();
                    workingPacket.RemoveRange(0, Platform.ByteSizeOfInt + parameterValueLength);


                    Parameters.Add(Encoding.UTF8.GetString(parameterKeyData).Trim(), new InstructionParameter(parameterValueData));
                }
                else
                {
                    parsing = false;
                }
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
            byte[] operationData = Encoding.UTF8.GetBytes(Operation.ToString());
            byteBuilder.AddRange(BitConverter.GetBytes(operationData.Length));
            byteBuilder.AddRange(operationData);

            // Handle Parameters
            foreach (string s in Parameters.Keys)
            {
                // Add key name
                byte[] keyData = Encoding.UTF8.GetBytes(s.Trim());
                byteBuilder.AddRange(BitConverter.GetBytes(keyData.Length));
                byteBuilder.AddRange(keyData);

                // Add key data
                byteBuilder.AddRange(Parameters[s].GetPacketBytesCountBytes());
                byteBuilder.AddRange(Parameters[s].GetPacketBytes());
            }

            return byteBuilder.ToArray();
        }

        public static Dictionary<string, InstructionParameter> CreateParametersDictionary(Dictionary<string, string> dictionary)
        {
            Dictionary<string, InstructionParameter> createdParameters = new Dictionary<string, InstructionParameter>();
            foreach (KeyValuePair<string, string> p in dictionary)
            {
                createdParameters.Add(p.Key, new InstructionParameter(p.Value));
            }
            return createdParameters;
        }
    }
}
