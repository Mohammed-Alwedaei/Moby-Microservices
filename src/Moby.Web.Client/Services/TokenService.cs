namespace Moby.Web.Client.Services;

public class TokenService : ITokenService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TokenService> _logger;

    public TokenService(HttpClient httpClient, ILogger<TokenService> logger)
    {
        _httpClient = httpClient; 
        _logger = logger;
    }

    /// <summary>
    /// Get token for the gateway
    /// </summary>
    /// <returns>TokenModel</returns>
    public async Task<TokenModel> GetTokenAsync()
    {
        try
        {
            _logger.LogInformation("Base address is: {address}", _httpClient.BaseAddress);

            var token = await _httpClient.GetFromJsonAsync<TokenModel>($"/api/GetToken");

            if (token.AccessToken is not null)
                return token;
        }
        catch (Exception exception)
        {
            _logger.LogWarning("Could not get access token for the targeted api, please try again later");
        }

        return new TokenModel();
    }
}
