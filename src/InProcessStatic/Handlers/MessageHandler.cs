using InProcessStatic.Models;

namespace InProcessStatic.Handlers;

public static class MessageHandler
{
    public static Message? Handle(NewMessage? newMessage)
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
