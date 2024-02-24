using IssueProvider.Application.Interfaces;
using Newtonsoft.Json;

namespace IssueProvider.Application.Models.GitLab;

public record IssueGitLabModel : IModel
{
    [JsonProperty("iid")] public string Id { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("state")] public string State { get; set; }
}