using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Moby.Services.Identity;

public static class SD
{
    public const string Admin = "Admin";
    public const string Customer = "Customer";

    public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Email(),
        new IdentityResources.Profile()
    };

    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
    {
        new ApiScope("mango", "Mango Server"),
        new ApiScope("read", "Read Data"),
        new ApiScope("write", "Write Data"),
        new ApiScope("delete", "Delete Data"),
    };

    public static IEnumerable<Client> Clients => new List<Client>
    {
        new Client
        {
            ClientId = "Client",
            ClientSecrets =
            {
                new Secret("secret".Sha512())
            },
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes = { "write", "read", "profile" }
        },
        new Client
        {
            ClientId = "Mango",
            ClientSecrets =
            {
                new Secret("secret".Sha512())
            },
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { "https://localhost:7085/signin-oidc" },
            PostLogoutRedirectUris = { "https://localhost:7085/signout-callback-oidc" },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                "remote_api",
                "mango"
            }
        }
    };
}