using IsolatedProcessIoC.Models;

namespace IsolatedProcessIoC.Handlers;

public class MessageHandler : IMessageHandler
{
    public Message? Handle(NewMessage? newMessage)
    {
        if (newMessage == null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(newMessage.Content))
        {
            return null;
        }

        var message = new Message(newMessage.Content);
        return message;
    }
}
