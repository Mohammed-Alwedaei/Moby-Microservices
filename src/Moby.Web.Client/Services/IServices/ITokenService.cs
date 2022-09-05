namespace Moby.Web.Client.Services.IServices;

public interface ITokenService
{
    Task<TokenModel> GetTokenAsync();
}
