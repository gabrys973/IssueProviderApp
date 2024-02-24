namespace IssueProvider.Application.Models;

public record IssueCommonExportModel
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string State { get; set; }
}