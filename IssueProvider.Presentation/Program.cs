using IssueProvider.Application.Interfaces;
using IssueProvider.Application.Services.GitHub;
using IssueProvider.Application.Services.GitLab;
using IssueProvider.Presentation.Providers.GitHub;
using IssueProvider.Presentation.Providers.GitLab;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IssueProvider.Presentation;

public sealed class Program
{
    public static async Task<int> Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((_, configuration) =>
            {
                configuration.SetBasePath(Directory.GetCurrentDirectory());
                configuration.AddJsonFile("appsettings.json", false, true);
                configuration.AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true,
                    true);
                configuration.AddEnvironmentVariables();
            })
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<MyApp>();
                services.AddKeyedScoped<IServiceBase, GitHubService>("github");
                services.AddKeyedScoped<IServiceBase, GitLabService>("gitlab");
                services.AddSingleton<GitHubProvider>();
                services.AddSingleton<GitLabProvider>();
            })
            .Build();

        var app = host.Services.GetRequiredService<MyApp>();

        var result = await app.StartAsync();
        return result;
    }
}