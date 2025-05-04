using System.Net.Http;
using Backend.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Backend.Tests.TestAPI;

public static class Backend
{
    public static HttpClient Launch()
    {
        using var context = BackendContext();
        context.Database.Migrate();

        return new WebApplicationFactory<DummyForAspNetTests>().CreateClient();
    }

    public static void CleanUp()
    {
        using var context = BackendContext();
        context.Database.EnsureDeleted();
    }

    private static BackendContext BackendContext()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        return new BackendContext(config);
    }
}