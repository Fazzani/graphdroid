namespace POC_GraphQL.Filters
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using POC_GraphQL.Common;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    public class PrincipalActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var userIdentity = GetPrincipal();
            context.HttpContext.User = new ClaimsPrincipal(userIdentity);
            var resultContext = await next();
        }

        public static ClaimsIdentity GetPrincipal()
        {
            const string Issuer = "https://seloger.tools";

            var claims = new List<Claim> {
                 new Claim(ClaimTypes.Name, "Andrew", ClaimValueTypes.String, Issuer),
                 new Claim(ClaimTypes.Surname, "Lock", ClaimValueTypes.String, Issuer),
                 new Claim(ClaimTypes.Country, "FR", ClaimValueTypes.String, Issuer),
                 new Claim(ClaimTypes.Role, Constants.Permissions.ADMIN, ClaimValueTypes.String, Issuer)
             };

            var userIdentity = new ClaimsIdentity(claims, "Passport");
            return userIdentity;
        }
    }
}
