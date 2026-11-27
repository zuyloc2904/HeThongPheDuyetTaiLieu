namespace DocumentService.Api.DTOs;

public class CreateDocumentRequest
{
    public string Title { get; set; } = string.Empty;

    public string? Content { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string? Approver { get; set; }
}