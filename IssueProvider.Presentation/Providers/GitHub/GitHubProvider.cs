using IssueProvider.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IssueProvider.Presentation.Providers.GitHub;

public sealed class GitHubProvider : ProviderBase
{
    public GitHubProvider([FromKeyedServices("github")] IServiceBase service)
    {
        _service = service;
    }
}