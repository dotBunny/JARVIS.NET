using System;
using System.Collections.Generic;
using JARVIS.Shared.Protocol;

namespace JARVIS.Shared.Services.Socket
{
    public static class DataHandler
    {
        public static void ProcessData(Sender session, List<byte> buffer, byte[] data, JCP protocol, ICommandFactory factory)
        {
            Log.Message("JCP", "Received " + data.Length + " bytes.");

            // We have to assume the buffer exists for the session
            buffer.AddRange(data);

            bool parsing = true;
            while (parsing)
            {
                if (buffer.Count >= Platform.ByteSizeOfInt)
                {
                    byte[] lengthBytes = buffer.GetRange(0, Platform.ByteSizeOfInt).ToArray();
                    int packetLength = BitConverter.ToInt32(lengthBytes, 0);

                    // We've got a complete packet at this point
                    if (buffer.Count >= (packetLength + Platform.ByteSizeOfInt))
                    {
                        Packet packet = new Packet(buffer.GetRange(Platform.ByteSizeOfInt, packetLength).ToArray(), protocol.EncryptionKey);

                        // Remove processed data
                        buffer.RemoveRange(0, Platform.ByteSizeOfInt + packetLength);

                        // Execute packet instructions
                        foreach (Instruction i in packet.GetInstructions())
                        {
                            Log.Message("socket", "Instruction Received -> " + i.Operation.ToString() + " from " + session.RemoteEndPoint.GetHost());

                            // Factory Pattern
                            ISocketCommand receivedCommand = factory.CreateCommand(i.Operation);


                            // Execute (decide what to do though)
                            if (receivedCommand.CanExecute(session))
                            {
                                receivedCommand.Execute(session, i.Parameters);
                            }
                        }

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
    }
}
