using IssueProvider.Application.Models.GitHub;
using IssueProvider.Application.Models.GitLab;
using IssueProvider.Presentation.MenuEnums;
using IssueProvider.Presentation.Providers.GitHub;
using IssueProvider.Presentation.Providers.GitLab;
using Spectre.Console;

namespace IssueProvider.Presentation;

public class MyApp
{
    private readonly GitHubProvider _gitHub;
    private readonly GitLabProvider _gitLab;

    public MyApp(GitHubProvider gitHub, GitLabProvider gitLab)
    {
        _gitHub = gitHub;
        _gitLab = gitLab;
    }

    public async Task<int> StartAsync()
    {
        ServiceMenuEnum serviceMenuEnum;

        do
        {
            serviceMenuEnum = AnsiConsole.Prompt(
                new SelectionPrompt<ServiceMenuEnum>()
                    .Title("[bold]Choose Service[/]")
                    .PageSize(10)
                    .AddChoices(ServiceMenuEnum.GitHub, ServiceMenuEnum.GitLab, ServiceMenuEnum.Exit));

            switch (serviceMenuEnum)
            {
                case ServiceMenuEnum.GitHub:
                    await _gitHub.GetIssueActions<IssueGitHubModel>();
                    break;
                case ServiceMenuEnum.GitLab:
                    await _gitLab.GetIssueActions<IssueGitLabModel>();
                    break;
            }
        } while (serviceMenuEnum != ServiceMenuEnum.Exit);

        return await Task.FromResult(0);
    }
}