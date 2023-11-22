using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;

namespace Ramboe.IS4;

public class Configuration
{
    //what apis do we need to secure?
    public static IEnumerable<ApiResource> GetApis() =>
        new List<ApiResource>
        {
            new ApiResource("MovieApi",
            new[]
            {
                ClaimTypes.Name
            })
        };

    public static IEnumerable<ApiScope> GetScopes() => new List<ApiScope>
    {
        new ApiScope("MovieApi", "MovieApiScopeDisplayName")
    };

    //what clients do have access to which APIs in which way?
    public static IEnumerable<Client> GetClients() =>
        new List<Client>
        {
            new Client
            {
                ClientId = "movieClient",

                ClientSecrets =
                {
                    new Secret("client_secret".ToSha256())
                },

                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                AllowedScopes =
                {
                    "MovieApi"
                },
                AllowOfflineAccess = true
            }
        };
}
