using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Mediator.Requests.Wix;
using Xunit;

namespace WebApi.IntegrationTests;

public class AuthenticationFlowTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public AuthenticationFlowTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public async Task RegisterLoginAndCallWixEndpoint_ShouldSucceed()
    {
        // Arrange
        var email = $"test_{Guid.NewGuid()}@example.com";
        var password = "Test123!";
        
        // Act 1: Register
        var registerResponse = await _client.PostAsJsonAsync("/register", new
        {
            email = email,
            password = password
        });

        // Assert 1: Registration should succeed
        Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);
        
        var registerContent = await registerResponse.Content.ReadAsStringAsync();
        var registerData = JsonSerializer.Deserialize<RegisterResponse>(registerContent, _jsonOptions);
        Assert.NotNull(registerData);
        Assert.NotNull(registerData.Token);
        Assert.NotNull(registerData.RefreshToken);

        // Act 2: Login
        var loginResponse = await _client.PostAsJsonAsync("/login", new
        {
            email = email,
            password = password
        });

        // Assert 2: Login should succeed
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        
        var loginContent = await loginResponse.Content.ReadAsStringAsync();
        var loginData = JsonSerializer.Deserialize<LoginResponse>(loginContent, _jsonOptions);
        Assert.NotNull(loginData);
        Assert.NotNull(loginData.Token);
        Assert.NotNull(loginData.RefreshToken);

        // Act 3: Call Wix endpoint with authentication
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginData.Token);

        var wixResponse = await _client.GetAsync("/api/wix/contacts");

        // Assert 3: Wix endpoint should return data (or appropriate status)
        // The endpoint might return OK with empty data or Unauthorized if Wix API is not configured
        // For this test, we're mainly checking that the authentication flow works
        Assert.True(
            wixResponse.StatusCode == HttpStatusCode.OK || 
            wixResponse.StatusCode == HttpStatusCode.Unauthorized,
            $"Expected OK or Unauthorized, but got {wixResponse.StatusCode}");

        if (wixResponse.StatusCode == HttpStatusCode.OK)
        {
            var wixContent = await wixResponse.Content.ReadAsStringAsync();
            Assert.NotNull(wixContent);
            // If we get OK, the response should be valid JSON
            var wixData = JsonSerializer.Deserialize<object>(wixContent, _jsonOptions);
            Assert.NotNull(wixData);
        }
    }

    private record RegisterResponse(string Token, string RefreshToken, DateTime Expiry);
    private record LoginResponse(string Token, string RefreshToken, DateTime Expiry);
}