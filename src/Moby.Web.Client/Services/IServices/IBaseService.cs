using Moby.Web.Client.Models;

namespace Moby.Web.Client.Services.IServices;

public interface IBaseService : IDisposable
{
    HttpRequestModel HttpRequest { get; set; }

    Task<T> SendAsync<T>(HttpRequestModel httpRequest);
}