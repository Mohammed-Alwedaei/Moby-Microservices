using System.Net.Http.Json;
using Moby.Web.Client.Services.IServices;
using Moby.Web.Shared.Models;

namespace Moby.Web.Client.Services;

public class TokenService : ITokenService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TokenService> _logger;
    private readonly ITokenService _tokenService;

    public TokenService(HttpClient httpClient, ILogger<TokenService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Token> GetTokenAsync(string targetApi)
    {
        try
        {
            _logger.LogInformation("Base address is: {address}", _httpClient.BaseAddress);

            var token = await _httpClient.GetFromJsonAsync<Token>($"/api/GetToken/{targetApi}");

            if (token.AccessToken is not null)
                return token;
        }
        catch (Exception exception)
        {
            _logger.LogWarning("Could not get access token for the targeted api {requestedApi}, please try again later", 
                targetApi);
        }

        return new Token();
    }
}
