namespace POC_GraphQL.Common
{
    using GraphQL.Authorization;
    using GraphQL.Conventions;
    using System.Security.Claims;
    public class GraphQLUserContext : IProvideClaimsPrincipal, IUserContext
    {
        public ClaimsPrincipal User { get; set; }
    }
}
