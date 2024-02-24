using IssueProvider.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IssueProvider.Presentation.Providers.GitLab;

public sealed class GitLabProvider : ProviderBase
{
    public GitLabProvider([FromKeyedServices("gitlab")] IServiceBase service)
    {
        _service = service;
    }
}