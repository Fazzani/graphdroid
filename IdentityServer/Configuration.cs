namespace IdentityServer
{
    using IdentityServer4.Models;
    using System.Collections.Generic;
    using System.Security.Claims;

    public static class Configuration
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
        {
            new ApiResource("graphqlApi", "GraphQL API")
        };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
        {
            new Client
            {
                ClientId = "graphqlApi",
                Claims = {
                    new Claim(ClaimTypes.Role, Common.Constants.Permissions.ADMIN),
                    new Claim(ClaimTypes.Sid, "1"),
                    new Claim(ClaimTypes.Surname, "Eric"),
                    new Claim(ClaimTypes.Country, "France"),
                    new Claim(ClaimTypes.Email, "eric@seloger.com"),
                    new Claim(ClaimTypes.GivenName, "Eric") },
                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("graphQLsecret".Sha256())
                },

                // scopes that client has access to
                AllowedScopes = { "graphqlApi" },
                AllowedCorsOrigins = new[] {
                    "http://localhost:53373",
                    "https://localhost:44362" }
            },
             new Client
            {
                ClientId = "graphqlApiViewer",
                Claims = {
                      new Claim(ClaimTypes.Role, Common.Constants.Permissions.READ_ONLY),
                    new Claim(ClaimTypes.Sid, "2"),
                    new Claim(ClaimTypes.Surname, "Mat"),
                    new Claim(ClaimTypes.Country, "USA"),
                    new Claim(ClaimTypes.Email, "mat@google.com"),
                    new Claim(ClaimTypes.GivenName, "Matt")},
                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("graphQLsecret2".Sha256())
                },

                // scopes that client has access to
                AllowedScopes = { "graphqlApi" },
                AllowedCorsOrigins = new[] {
                    "http://localhost:53373",
                    "https://localhost:44362" }
            }
        };
        }
    }

}
