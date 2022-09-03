using Moby.Web.Client.Services.IServices;
using System.Net.Http.Headers;
using System.Net.Http;
using Moby.Web.Shared.Models;

namespace Moby.Web.Client.Services;

public class BaseService : IBaseService
{
    public IHttpClientFactory ClientFactory { get; set; }

    public ITokenService TokenService { get; set; }

    public BaseService(IHttpClientFactory httpClientFactory, ITokenService tokenService)
    {
        ClientFactory = httpClientFactory;
        TokenService = tokenService;
    }

    public async Task<HttpClient> HttpClient(string baseUrl, string service)
    {
        var client = ClientFactory.CreateClient("HttpClient");

        client.BaseAddress = new Uri(baseUrl);

        var accessToken = await GetAccessTokenAsync(service);

        client.DefaultRequestHeaders.Authorization = new 
            AuthenticationHeaderValue(accessToken.TokenType, accessToken.AccessToken);

        return client;
    }

    public async Task<Token> GetAccessTokenAsync(string service)
    {
        var tokenDto = await TokenService.GetTokenAsync(service);

        return tokenDto;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
