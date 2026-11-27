namespace DocumentService.Api.DTOs;

public class UpdateDocumentRequest
{
    public string Title { get; set; } = string.Empty;

    public string? Content { get; set; }

    public string? Approver { get; set; }
}