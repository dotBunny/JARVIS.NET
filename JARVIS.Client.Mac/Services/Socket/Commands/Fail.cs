using System.Collections.Generic;
using JARVIS.Client.Mac;
            
namespace JARVIS.Client.Mac.Services.Socket.Commands
{
    public class Fail : JARVIS.Shared.Services.Socket.ISocketCommand
    {
        public bool CanExecute()
        {
            return true;
        }
        public void Execute(Sender session, Dictionary<string, string> parameters)
        {
            Shared.Log.Message("AUTH","Failed");
        }
    }
}