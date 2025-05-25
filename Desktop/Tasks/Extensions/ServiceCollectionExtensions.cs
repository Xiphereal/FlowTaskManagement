using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop.Tasks.Extensions;

public static class ServiceCollectionExtensions
{
    private const string BackendUriSectionName = "BackendUri";

    public static void AddBackendTaskRepository(
        this ServiceCollection services,
        IConfigurationRoot config)
    {
        if (config.GetSection(BackendUriSectionName).Value is null)
            throw new ArgumentException(
                "A 'BackendUri' is expected to be defined in order to communicate with the Backend");

        services.AddSingleton<HttpClient>(_ =>
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress =
                new Uri(config.GetSection(BackendUriSectionName).Value!);

            return httpClient;
        });
        services.AddSingleton<ITaskRepository, BackendTaskRepository>();
    }
}