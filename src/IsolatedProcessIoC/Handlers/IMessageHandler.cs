using IsolatedProcessIoC.Models;

namespace IsolatedProcessIoC.Handlers;

public interface IMessageHandler
{
    Message? Handle(NewMessage? newMessage);
}
