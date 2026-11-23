namespace DocumentService.Core.Models;

public class Notification
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid DocumentId { get; set; }

    public string Recipient { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string EventType { get; set; } = string.Empty;

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Document? Document { get; set; }
}