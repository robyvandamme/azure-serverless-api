namespace IsolatedProcessIoC.Models;

public record NewMessage
{
    public NewMessage(string content)
    {
        Content = content;
    }

    public string Content { get; init; }
}
