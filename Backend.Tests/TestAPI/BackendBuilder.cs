using System;
using System.Collections.Generic;
using System.Net.Http;
using Backend.Domain;
using Backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Backend.Tests.TestAPI;

public class BackendBuilder
{
    private readonly BackendContext context;
    private Guid databaseId = Guid.NewGuid();
    private bool alwaysThrowingAnyException;

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

    public BackendBuilder With(Guid id)
    {
        databaseId = id;

        return this;
    }

    public BackendBuilder AlwaysThrowingAnyException()
    {
        alwaysThrowingAnyException = true;

        return this;
    }

    public HttpClient Launch()
    {
        context.SaveChanges();

        if (alwaysThrowingAnyException)
            return null;

        return new CustomWebAppFactory(BuildConnectionString())
            .CreateClient();
    }

    private string BuildConnectionString()
    {
        // Having a different name for the database allows for making tests persistence
        // independent of one another.
        return $"Data Source=TestDb_{databaseId}.db";
    }

    private BackendContext BackendContext()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    ["ConnectionStrings:BackendDatabase"] = BuildConnectionString()
                })
            .Build();

        return new BackendContext(config);
    }
}