﻿using System.Text;
using IssueProvider.Application.Models;
using IssueProvider.Application.Models.GitHub;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IssueProvider.Application.Services.GitHub;

public class GitHubService : Servicebase
{
    public GitHubService(IConfiguration configuration) : base(configuration)
    {
        _baseUri = "https://api.github.com/repos/" + _configuration["GitHub:Owner"] + "/" +
                   _configuration["GitHub:Repository"] + "/issues";
        _client = CreateClient(_configuration["GitHub:Key"]);
    }

    public override async Task<string?> CreateNewIssueAsync(string title, string description)
    {
        try
        {
            var requestBody = new PostModel
            {
                Title = title,
                Description = description
            };
            var jsonContent = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_baseUri, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseContent);

            var id = jObject.Value<string>("number");

            SuccessMessage();
            return id;
        }
        catch (Exception e)
        {
            ErrorMessage();
            return default;
        }
    }

    public override async Task EditIssueAsync(string id)
    {
        try
        {
            var issue = await GetByIdIssueAsync<IssueGitHubModel>(id);

            Console.Clear();
            Console.WriteLine("Set new title for issue(leave empty if don't want to change):");
            var newTitle = Console.ReadLine();
            newTitle = string.IsNullOrEmpty(newTitle) ? issue.Title : newTitle;

            Console.WriteLine("Set new description for issue(leave empty if don't want to change):");
            var newDescription = Console.ReadLine();
            newDescription = string.IsNullOrEmpty(newDescription) ? issue.Description : newDescription;

            var requestBody = new PatchModel
            {
                Title = newTitle,
                Description = newDescription,
                State = issue.State
            };

            var jsonContent = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PatchAsync(_baseUri + '/' + id, content);
            response.EnsureSuccessStatusCode();

            SuccessMessage();
        }
        catch (Exception e)
        {
            ErrorMessage();
        }
    }

    public override async Task CloseIssueAsync(string id)
    {
        try
        {
            var issue = await GetByIdIssueAsync<IssueGitHubModel>(id);

            var requestBody = new PatchModel
            {
                Title = issue.Title,
                Description = issue.Description,
                State = "close"
            };

            var jsonContent = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PatchAsync(_baseUri + '/' + id, content);
            response.EnsureSuccessStatusCode();

            SuccessMessage();
        }
        catch (Exception e)
        {
            ErrorMessage();
        }
    }

    public override async Task ExportIssuesAsync()
    {
        Console.Clear();
        Console.WriteLine("Input file name with extension:");
        var fileName = Console.ReadLine();

        try
        {
            var response = await GetAllIssuesAsync<IssueGitHubModel>();

            var issues = response.Select(x => new IssueCommonExportModel
            {
                Title = x.Title,
                Description = x.Description,
                State = x.State.Equals("close") ? "close" : "open"
            });

            var result = JsonConvert.SerializeObject(issues);

            await File.WriteAllTextAsync(fileName + ".txt", result);

            SuccessMessage();
        }
        catch (Exception e)
        {
            ErrorMessage();
        }
    }

    public override async Task ImportIssuesAsync()
    {
        Console.Clear();
        Console.WriteLine("Input file name with extension:");
        var fileName = Console.ReadLine();

        try
        {
            var fileContent = await File.ReadAllTextAsync(fileName + ".txt");

            var issues = JsonConvert.DeserializeObject<IList<IssueCommonExportModel>>(fileContent);

            var TResult = issues.Select(x => new IssueCommonExportModel
                { Title = x.Title, Description = x.Description, State = x.State });

            foreach (var result in TResult)
            {
                var id = await CreateNewIssueAsync(result.Title, result.Description);
                if (result.State.Equals("close"))
                    await CloseIssueAsync(id);
            }

            SuccessMessage();
        }
        catch (Exception e)
        {
            ErrorMessage();
        }
    }
}