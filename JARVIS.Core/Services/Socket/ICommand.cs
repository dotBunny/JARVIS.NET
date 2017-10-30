using JARVIS.Shared.Services.Socket;
namespace JARVIS.Core.Services.Socket
{
    public interface ICommand
    {
        void ExecuteCommand(Sender session, Protocol.Packet packet);
    }
}
