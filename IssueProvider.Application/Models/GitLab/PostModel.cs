using Newtonsoft.Json;

namespace IssueProvider.Application.Models.GitLab;

public record PostModel
{
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
}