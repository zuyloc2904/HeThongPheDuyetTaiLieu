namespace DocumentService.Core.Models;

public class Document
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string? Content { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string? Approver { get; set; }

    public string Status { get; set; } = "Draft";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<ApprovalHistory> ApprovalHistories { get; set; } = new();

    public List<Notification> Notifications { get; set; } = new();
}