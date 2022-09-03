using Moby.Web.Shared.Models;

namespace Moby.Web.Client.Services.IServices;

public interface IBaseService : IDisposable
{
    Task<HttpClient> HttpClient(string baseUrl, string service);

    Task<Token> GetAccessTokenAsync(string service);
}