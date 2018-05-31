using NQRW.Messaging;
using NQRW.Messaging.Messages;

namespace NQRW.FiniteStateMachine.Commands
{
    public interface IStateCommand: IMessage
    {

    }

    public abstract class BaseStateCommand : BaseMessage, IStateCommand
    {

    }
}
