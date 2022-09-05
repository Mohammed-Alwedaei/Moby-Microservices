namespace Moby.Web.Client.Services;

public class BaseService : IBaseService
{
    public IHttpClientFactory ClientFactory { get; set; }

    public ITokenService TokenService { get; set; }

    public IConfiguration Configuration { get; set; }

    public BaseService(IHttpClientFactory httpClientFactory, ITokenService tokenService, IConfiguration configuration)
    {
        ClientFactory = httpClientFactory;
        TokenService = tokenService;
        Configuration = configuration;
    }

    /// <summary>
    /// Create HttpClient
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <returns>HttpClient</returns>
    public async Task<HttpClient> HttpClient()
    {
        var client = ClientFactory.CreateClient("HttpClient");

        var baseUrl = Configuration.GetValue<string>("GatewayUrl");

        client.BaseAddress = new Uri(baseUrl);

        var accessToken = await GetAccessTokenAsync();

        client.DefaultRequestHeaders.Authorization = new 
            AuthenticationHeaderValue(accessToken.TokenType, accessToken.AccessToken);

        return client;
    }

    /// <summary>
    /// Get Access token
    /// </summary>
    /// <returns>TokenModel</returns>
    public async Task<TokenModel> GetAccessTokenAsync()
    {
        var tokenDto = await TokenService.GetTokenAsync();

        return tokenDto;
    }

    /// <summary>
    /// Dispose this
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
