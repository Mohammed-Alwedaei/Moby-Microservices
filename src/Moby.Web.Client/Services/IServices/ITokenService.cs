using Moby.Web.Shared.Models;

namespace Moby.Web.Client.Services.IServices;

public interface ITokenService
{
    Task<Token> GetTokenAsync(string targetApi);
}
