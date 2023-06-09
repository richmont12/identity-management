using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
        };


    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope(name: "dataapi1", displayName: "Data Api 1"),
            new ApiScope(name: "dataapi2", displayName: "Data Api 2")
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource(name: "dataapi1", displayName: "Data Api 1")
            {
                Scopes = { "dataapi1" }
            },
            new ApiResource(name: "dataapi2", displayName: "Data Api 2")
            {
                Scopes = { "dataapi2" }
            }
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "machineclient",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                // scopes that client has access to
                AllowedScopes = { "dataapi1" }
            },
            new Client
            {
                ClientId = "machineclientdelegation",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = new[] { OidcConstants.GrantTypes.TokenExchange },

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                // scopes that client has access to
                AllowedScopes = { "dataapi2" }
            },
            new Client
            {
                ClientId = "webclient",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                // where to redirect to after login
                RedirectUris = { "http://localhost:3000/api/auth/callback/identity-provider" },

                // where to redirect to after logout
                PostLogoutRedirectUris =
                {
                    "http://localhost:3000"
                },

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "dataapi1"
                }
            }
        };
}