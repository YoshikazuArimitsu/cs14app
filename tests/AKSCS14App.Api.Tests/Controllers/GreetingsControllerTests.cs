using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CS14App.Api.Tests.Controllers;

[Collection(ApiTestCollection.Name)]
public class GreetingsControllerTests
{
    private readonly HttpClient _client;

    public GreetingsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_WithValidName_ReturnsOkWithGreeting()
    {
        var response = await _client.GetAsync("/api/greetings/Taro");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("Taro", body);
    }

    [Fact]
    public async Task Get_WithWhitespaceName_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/greetings/%20");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
