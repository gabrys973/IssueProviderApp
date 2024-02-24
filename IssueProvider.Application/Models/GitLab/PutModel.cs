using Newtonsoft.Json;

namespace IssueProvider.Application.Models.GitLab;

public record PutModel
{
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("state_event")] public string? State { get; set; }
}