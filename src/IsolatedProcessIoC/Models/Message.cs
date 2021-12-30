using System;

namespace IsolatedProcessIoC.Models;

public record Message
{
    public Message(string content)
    {
        Id = Guid.NewGuid();
        Content = content;
        CreatedOn = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; init; }

    public string Content { get; init; }

    public DateTimeOffset CreatedOn { get; init; }
}
