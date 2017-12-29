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
                if (buffer.Count >= JCP.SizeOfLength)
                {
                    byte[] lengthBytes = buffer.GetRange(0, JCP.SizeOfLength).ToArray();
                    int packetLength = BitConverter.ToInt32(lengthBytes, 0);

                    // We've got a complete packet at this point
                    if (buffer.Count >= (packetLength + JCP.SizeOfLength))
                    {
                        Packet packet = new Packet(buffer.GetRange(JCP.SizeOfLength, packetLength).ToArray(), protocol.EncryptionKey);

                        // Remove processed data
                        buffer.RemoveRange(0, JCP.SizeOfLength + packetLength);

                        // Execute packet instructions
                        foreach (Instruction i in packet.GetInstructions())
                        {
                            Log.Message("socket", "Instruction Received -> " + i.Operation.ToString() + " from " + session.RemoteEndPoint.GetHost());

                            // Factory Pattern
                            ISocketCommand receivedCommand = factory.CreateCommand(i.Operation);


                            // Execute (decide what to do though)
                            if (receivedCommand.CanExecute())
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


           

                








            //// Terminator is the position, 0 being the first character
            //int terminator = buffer.IndexOf(JCP.TransmissionTerminator);

            //// Only process when we know we've got a complete package (and something to look at)
            //while (terminator != -1 && buffer.Count >= 1)
            //{
                //Log.Message("JCP", "Read packet from 0 to " + terminator);

                //Packet[] packets = protocol.GetPackets(buffer.GetRange(0, terminator).ToArray());

                //Log.Message("Socket", "Process Packets");

                //foreach (Packet p in packets)
                //{
                //    foreach (Instruction i in p.GetInstructions())
                //    {
                //        Log.Message("socket", "Instruction Received -> " + i.Operation.ToString() + " from " + session.RemoteEndPoint.GetHost());

                //        // Factory Pattern
                //        ISocketCommand receivedCommand = factory.CreateCommand(i.Operation);


                //        // Execute (decide what to do though)
                //        if (receivedCommand.CanExecute())
                //        {
                //            receivedCommand.Execute(session, i.Parameters);
                //        }
                //    }
                //}

                //buffer.RemoveRange(0, terminator + 1);

                //// Look again
                //terminator = buffer.IndexOf(JCP.TransmissionTerminator);

        }
    }
}
