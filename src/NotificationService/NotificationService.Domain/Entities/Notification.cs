namespace NotificationService.Domain.Entities;

public class Notification
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private Notification() { } // EF Core

    public Notification(Guid userId, string title, string message)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message is required.", nameof(message));

        Id = Guid.NewGuid();
        UserId = userId;
        Title = title;
        Message = message;
        CreatedAt = DateTime.UtcNow;
    }
}
