namespace POC_GraphQL.Filters
{
    using GraphQL.Conventions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using POC_GraphQL.Common;
    using System.Security.Claims;
    using System.Threading.Tasks;
    public class PrincipalActionFilterAttribute : ActionFilterAttribute
    {
        readonly string _role;
        public PrincipalActionFilterAttribute(string role = Constants.Permissions.READ_ONLY)
        {
            _role = role;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userIdentity = PrincipalActionFilter.GetPrincipal();
            var userContext = context.HttpContext.RequestServices.GetService(typeof(IUserContext)) as GraphQLUserContext;

            userContext.User = context.HttpContext.User = new ClaimsPrincipal(userIdentity);
            
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
