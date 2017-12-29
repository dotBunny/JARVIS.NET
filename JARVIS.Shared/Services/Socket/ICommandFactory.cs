
using JARVIS.Shared.Protocol;

namespace JARVIS.Shared.Services.Socket
{
    public interface ICommandFactory
    {
        ISocketCommand CreateCommand(Instruction.OpCode commandType);
    }
}
