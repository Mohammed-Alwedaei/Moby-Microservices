namespace Moby.Web.Client.Services.IServices;

public interface IBaseService : IDisposable
{
    Task<HttpClient> HttpClient();

    Task<TokenModel> GetAccessTokenAsync();
}