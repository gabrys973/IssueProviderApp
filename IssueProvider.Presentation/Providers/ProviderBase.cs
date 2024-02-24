using IssueProvider.Application.Interfaces;
using IssueProvider.Presentation.MenuEnums;
using Spectre.Console;

namespace IssueProvider.Presentation.Providers;

public abstract class ProviderBase
{
    protected IServiceBase _service;

    internal async Task GetIssueActions<T>() where T : IModel
    {
        IssueActionMenuEnum issueActionEnum;

        do
        {
            Console.Clear();
            issueActionEnum = AnsiConsole.Prompt(
                new SelectionPrompt<IssueActionMenuEnum>()
                    .Title("[bold]Action menu[/]")
                    .PageSize(10)
                    .AddChoices(IssueActionMenuEnum.Create, IssueActionMenuEnum.Edit, IssueActionMenuEnum.Close,
                        IssueActionMenuEnum.Export,
                        IssueActionMenuEnum.Import, IssueActionMenuEnum.Exit));

            switch (issueActionEnum)
            {
                case IssueActionMenuEnum.Create:
                    await CreateCase();
                    break;

                case IssueActionMenuEnum.Edit:
                    await EditCase<T>();
                    break;

                case IssueActionMenuEnum.Close:
                    await CloseCase<T>();
                    break;

                case IssueActionMenuEnum.Export:
                    await _service.ExportIssuesAsync();
                    WaitAfterAciton();
                    break;

                case IssueActionMenuEnum.Import:
                    await _service.ImportIssuesAsync();
                    WaitAfterAciton();
                    break;
            }
        } while (issueActionEnum != IssueActionMenuEnum.Exit);
    }

    private async Task CreateCase()
    {
        Console.Clear();
        Console.WriteLine("Set title for new issue:");
        var title = Console.ReadLine();
        Console.WriteLine("Set description for new issue:");
        var description = Console.ReadLine();

        await _service.CreateNewIssueAsync(title, description);

        WaitAfterAciton();
    }

    private async Task EditCase<T>() where T : IModel
    {
        var issues = await _service.GetAllIssuesAsync<T>();
        if (issues is null || !issues.Any())
            return;

        var issue = AnsiConsole.Prompt(
            new SelectionPrompt<T>()
                .Title("[bold]Action menu[/]")
                .PageSize(10)
                .AddChoices(issues).UseConverter(model =>
                    "Id: " + model.Id + ", Title: " + model.Title + ", Description: " + model.Description));

        await _service.EditIssueAsync(issue.Id);

        WaitAfterAciton();
    }

    private async Task CloseCase<T>() where T : IModel
    {
        var issues = await _service.GetAllIssuesAsync<T>();
        if (issues is null || !issues.Any())
            return;

        var issue = AnsiConsole.Prompt(
            new SelectionPrompt<T>()
                .Title("Action menu")
                .PageSize(10)
                .AddChoices(issues).UseConverter(model =>
                    "Id: " + model.Id + ", Title: " + model.Title + ", Description: " + model.Description));

        await _service.CloseIssueAsync(issue.Id);

        WaitAfterAciton();
    }

    private void WaitAfterAciton()
    {
        Console.WriteLine("Enter any key to continue..");
        Console.ReadKey();
    }
}