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
                    new Claim("role", Common.Constants.Policies.AdminPolicy),
                    new Claim("Id", "1")},
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
                    new Claim("role", Common.Constants.Policies.ViewerPolicy),
                    new Claim("Id", "2")},
                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("graphQLsecret2".Sha256())
                },

                // scopes that client has access to
                AllowedScopes = { "graphqlApiViewer" },
                AllowedCorsOrigins = new[] {
                    "http://localhost:53373",
                    "https://localhost:44362" }
            }
        };
        }
    }

}
