using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Backend.Tests.TestAPI;

public static class Backend
{
    public static HttpClient Launch()
    {
        return new WebApplicationFactory<DummyForAspNetTests>().CreateClient();
    }
}