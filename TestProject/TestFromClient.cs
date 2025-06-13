using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(); // Создает в памяти сервер и клиент
    }

    [Fact]
    public async Task Get_HomePage_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Post_Email_ReturnsOk()
    {
        var payload = new { email = "test@example.com" };

        var response = await _client.PostAsJsonAsync("/api/email", payload);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("success", content);
    }
}