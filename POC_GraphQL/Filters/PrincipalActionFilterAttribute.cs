namespace POC_GraphQL.Filters
{
    using global::Common;
    using GraphQL.Conventions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using POC_GraphQL.Common;
    using System;
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
            var userContext = context.HttpContext.RequestServices.GetService(typeof(IUserContext)) as GraphQLUserContext;

            //Mock authentification
            //var userIdentity = PrincipalActionFilter.GetPrincipal();
            //userContext.User = context.HttpContext.User = new ClaimsPrincipal(userIdentity);

            //JWT authentification
            userContext.User = context.HttpContext.User; 

            return base.OnActionExecutionAsync(context, next);
        }

    }
}
