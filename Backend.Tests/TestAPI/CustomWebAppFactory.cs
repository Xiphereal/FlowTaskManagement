using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Backend.Tests.TestAPI;

public class CustomWebAppFactory : WebApplicationFactory<DummyForAspNetTests>
{
    private readonly string connectionString;

    public CustomWebAppFactory(string connectionString)
    {
        this.connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // This ensures the Backend ConnectionStrings:BackendDatabase can be configured from
        // tests. 
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    ["ConnectionStrings:BackendDatabase"] = connectionString
                });
        });
    }
}