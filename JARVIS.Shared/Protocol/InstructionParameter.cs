using System;
using System.Collections.Generic;

namespace JARVIS.Shared.Protocol
{
    /// <summary>
    /// JCP Instruction Parameter
    /// </summary>
    public class InstructionParameter
    {
        /// <summary>
        /// An indicator that the value of the parameter is a byte array
        /// </summary>
        const byte ByteTypeMarker = 0x00;

        /// <summary>
        /// An indicator that the value of the parameter is a string
        /// </summary>
        const byte StringTypeMarker = 0x01;

        bool _cachedIsString = false;
        string _cachedString = string.Empty;
        byte[] _cachedData;
        int _cachedDataLength = 0;
        byte[] _cachedDataLengthData;
        byte[] _cachedPacketBytesLengthData;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Shared.Protocol.InstructionParameter"/> class from a byte set.
        /// </summary>
        /// <param name="data">Data.</param>
        public InstructionParameter(byte[] data)
        {
            byte marker = data[0];

            _cachedDataLength = data.Length - 1;
            _cachedData = new byte[_cachedDataLength];
            Array.Copy(data, 1, _cachedData, 0, _cachedDataLength);
            _cachedDataLengthData = BitConverter.GetBytes(_cachedDataLength);
            _cachedPacketBytesLengthData = BitConverter.GetBytes(_cachedDataLength + 1);

            // Raw binary
            if ( marker == ByteTypeMarker)
            {
                _cachedIsString = false;
                _cachedString = string.Empty;
            }
            else
            {
                // no need to check, treat as string
                _cachedIsString = true;
                if (_cachedIsString)
                {
                    _cachedString = System.Text.Encoding.UTF8.GetString(_cachedData);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Shared.Protocol.InstructionParameter"/> class from a string value.
        /// </summary>
        /// <param name="data">The string value.</param>
        public InstructionParameter(string data)
        {
            _cachedData = System.Text.Encoding.UTF8.GetBytes(data);
            _cachedDataLength = _cachedData.Length;
            _cachedDataLengthData = BitConverter.GetBytes(_cachedDataLength);
            _cachedPacketBytesLengthData = BitConverter.GetBytes(_cachedDataLength + 1);
            _cachedString = data;
            _cachedIsString = true;
        }

        /// <summary>
        /// Is the parameter actually a string?
        /// </summary>
        /// <returns><c>true</c>, if it is a string, <c>false</c> otherwise.</returns>
        public bool IsString()
        {
            return _cachedIsString;
        }

        /// <summary>
        /// Provides a string representation of the data, or the actual string if it is a string.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:JARVIS.Shared.Protocol.InstructionParameter"/>.</returns>
        public override string ToString()
        {
            if (!_cachedIsString)
            {
                return _cachedDataLength.ToString() + " Bytes";
            }
            return _cachedString;    
        }

        /// <summary>
        /// Gets the length of the byte data of the instruction parameter.
        /// </summary>
        /// <returns>The byte count.</returns>
        public int GetByteCount()
        {
            return _cachedDataLength;
        }

        /// <summary>
        /// Gets the bytes of the length of the byte data of the instruction parameter.
        /// </summary>
        /// <returns>The byte count bytes.</returns>
        public byte[] GetByteCountBytes()
        {
            return _cachedDataLengthData;
        }

        /// <summary>
        /// Get the byte data of the instruction parameter.
        /// </summary>
        /// <returns>The value bytes.</returns>
        public byte[] GetBytes()
        {
            return _cachedData;
        }

        /// <summary>
        /// Gets the bytes used when transmitting (includes a type marker) via JCP.
        /// </summary>
        /// <returns>The packet bytes.</returns>
        public byte[] GetPacketBytes()
        {
            List<byte> returnData = new List<byte>();

            if ( _cachedIsString)
            {
                returnData.Add(StringTypeMarker);
            }
            else
            {
                returnData.Add(ByteTypeMarker);
            }

            returnData.AddRange(_cachedData);

            return returnData.ToArray();
        }

        /// <summary>
        /// Gets the bytes of the length of the packet version (marked) of the values byte data.
        /// </summary>
        /// <returns>The packet bytes count bytes.</returns>
        public byte[] GetPacketBytesCountBytes()
        {
            return _cachedPacketBytesLengthData;
        }

        public static InstructionParameter CreateFromUnmarkedBytes(byte[] bytes)
        {
            byte[] newValues = new byte[bytes.Length + 1];
            newValues[0] = ByteTypeMarker;  
            Array.Copy(bytes, 0, newValues, 1, bytes.Length);
            return new InstructionParameter(newValues);
        }
    }
}