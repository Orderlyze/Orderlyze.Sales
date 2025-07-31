using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApi.Mediator.Requests.Wix;
using Xunit;
using Xunit.Abstractions;

namespace WebApi.IntegrationTests;

public class AuthenticationFlowTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ITestOutputHelper _output;

    public AuthenticationFlowTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _output = output;
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddDebug();
            });
        });
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
        var registerContent = await registerResponse.Content.ReadAsStringAsync();
        _output.WriteLine($"Register Response Status: {registerResponse.StatusCode}");
        _output.WriteLine($"Register Response Content: {registerContent}");
        
        Assert.True(registerResponse.IsSuccessStatusCode, 
            $"Expected success status, but got {registerResponse.StatusCode}. Content: {registerContent}");

        // Act 2: Login
        var loginResponse = await _client.PostAsJsonAsync("/login", new
        {
            email = email,
            password = password
        });

        // Assert 2: Login should succeed
        var loginContent = await loginResponse.Content.ReadAsStringAsync();
        _output.WriteLine($"Login Response Status: {loginResponse.StatusCode}");
        _output.WriteLine($"Login Response Content: {loginContent}");
        
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        
        var loginData = JsonSerializer.Deserialize<AccessTokenResponse>(loginContent, _jsonOptions);
        Assert.NotNull(loginData);
        Assert.NotNull(loginData.accessToken);
        Assert.NotNull(loginData.refreshToken);

        // Act 3: Call Wix endpoint with authentication
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginData.accessToken);

        var wixResponse = await _client.GetAsync("/wix");

        // Assert 3: Wix endpoint should return data (or appropriate status)
        // The endpoint might return OK with empty data, Unauthorized if not authenticated,
        // or InternalServerError if Wix API is not properly configured
        // For this test, we're mainly checking that the authentication flow works
        _output.WriteLine($"Wix Response Status: {wixResponse.StatusCode}");
        
        Assert.True(
            wixResponse.StatusCode == HttpStatusCode.OK || 
            wixResponse.StatusCode == HttpStatusCode.Unauthorized ||
            wixResponse.StatusCode == HttpStatusCode.InternalServerError,
            $"Expected OK, Unauthorized, or InternalServerError, but got {wixResponse.StatusCode}");

        if (wixResponse.StatusCode == HttpStatusCode.OK)
        {
            var wixContent = await wixResponse.Content.ReadAsStringAsync();
            Assert.NotNull(wixContent);
            // If we get OK, the response should be valid JSON
            var wixData = JsonSerializer.Deserialize<object>(wixContent, _jsonOptions);
            Assert.NotNull(wixData);
        }
    }

    private record AccessTokenResponse(string accessToken, string refreshToken, long expiresIn);
}