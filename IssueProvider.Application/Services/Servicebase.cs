using System.Net.Http.Headers;
using IssueProvider.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace IssueProvider.Application.Services;

public abstract class Servicebase : IServiceBase
{
    protected string _baseUri;
    protected HttpClient _client;
    protected readonly IConfiguration _configuration;

    public Servicebase(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public abstract Task CreateNewIssueAsync(string title, string description);
    public abstract Task EditIssueAsync(string id);
    public abstract Task CloseIssueAsync(string id);
    public abstract Task ExportIssuesAsync();
    public abstract Task ImportIssuesAsync();

    public async Task<IList<T>?> GetAllIssuesAsync<T>() where T : IModel
    {
        try
        {
            var response = await _client.GetAsync(_baseUri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IList<T>>(content);

            return result;
        }
        catch (Exception e)
        {
            ErrorMessage();
            return default;
        }
    }

    public async Task<T?> GetByIdIssueAsync<T>(string id) where T : IModel
    {
        try
        {
            var response = await _client.GetAsync(_baseUri + '/' + id);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<T>(content);

            return result;
        }
        catch (Exception e)
        {
            ErrorMessage();
            return default;
        }
    }

    public HttpClient CreateClient(string apiToken)
    {
        var client = new HttpClient();
        var token = apiToken;

        client.DefaultRequestHeaders.UserAgent.Add(
            new ProductInfoHeaderValue("AppName", "1.0"));
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        return client;
    }

    protected void SuccessMessage()
    {
        Console.WriteLine("\n\nSuccess");
    }

    protected void ErrorMessage()
    {
        Console.WriteLine("\n\nError");
        Console.WriteLine("Something went wrong");
    }
}