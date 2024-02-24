namespace IssueProvider.Application.Interfaces;

public interface IServiceBase
{
    public Task CreateNewIssueAsync(string title, string description);
    public Task EditIssueAsync(string id);
    public Task CloseIssueAsync(string id);
    public Task ExportIssuesAsync();
    public Task ImportIssuesAsync();
    public Task<IList<T>?> GetAllIssuesAsync<T>() where T : IModel;
    public Task<T> GetByIdIssueAsync<T>(string id) where T : IModel;

    protected HttpClient CreateClient(string apiToken);
}