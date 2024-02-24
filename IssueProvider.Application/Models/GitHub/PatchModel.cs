using Newtonsoft.Json;

namespace IssueProvider.Application.Models.GitHub;

public record PatchModel
{
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("body")] public string? Description { get; set; }
    [JsonProperty("state")] public string? State { get; set; }
}