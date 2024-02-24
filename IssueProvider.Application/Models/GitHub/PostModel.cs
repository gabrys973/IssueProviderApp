using Newtonsoft.Json;

namespace IssueProvider.Application.Models.GitHub;

public record PostModel
{
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("body")] public string? Description { get; set; }
}