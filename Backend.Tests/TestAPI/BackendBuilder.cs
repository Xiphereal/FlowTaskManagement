using System.Net.Http;
using Backend.Domain;
using Backend.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Backend.Tests.TestAPI;

public class BackendBuilder
{
    private readonly BackendContext context;

    private BackendBuilder()
    {
        context = BackendContext();
        context.Database.Migrate();
    }

    public static BackendBuilder Backend()
    {
        return new BackendBuilder();
    }

    public BackendBuilder With(Task task)
    {
        context.Add(task);

        return this;
    }

    public HttpClient Launch()
    {
        context.SaveChanges();

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