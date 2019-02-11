namespace POC_GraphQL.Controllers
{
    using global::Common;
    using GraphQL;
    using GraphQL.Authorization;
    using GraphQL.Conventions;
    using GraphQL.Http;
    using GraphQL.Types;
    using GraphQL.Validation;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using POC_GraphQL.Common;
    using POC_GraphQL.Filters;
    using POC_GraphQL.Queries;
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    [Route("[controller]"),
     PrincipalActionFilter(Constants.Permissions.READ_ONLY), 
     Authorize]
    public class GraphQLController : Controller
    {
        private readonly ISchema _schema;
        private readonly IDocumentExecuter _documentExecuter;
        private readonly IDocumentWriter _documentWriter;
        private readonly IUserContext _userContext;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<GraphQLController> _logger;
        private readonly IServiceProvider _serviceProvider;
        public GraphQLController(ISchema schema,
            IDocumentExecuter documentExecuter,
            IDocumentWriter documentWriter,
            IUserContext userContext,
            ILoggerFactory logger,
            IServiceProvider serviceProvider)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
            _loggerFactory = logger;
            _logger = _loggerFactory.CreateLogger<GraphQLController>();
            _userContext = userContext;
            _documentWriter = documentWriter;
            _serviceProvider = serviceProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs,
                UserContext = _userContext
            };

            var validationRuleServices = _serviceProvider.GetServices<IValidationRule>();
            executionOptions.ValidationRules = DocumentValidator.CoreRules().Concat(validationRuleServices);

            //Using middleware example
            executionOptions.FieldMiddleware.Use(next =>
            {
                return context =>
                {
                    _logger.LogDebug($"FieldName middleware => {context.FieldName}");
                    return next(context);
                };
            });

            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            HttpStatusCode statusCode = HttpStatusCode.OK;

            if (result.Errors?.Any() ?? false)
            {
                statusCode = HttpStatusCode.InternalServerError;
                if (result.Errors.Any(x => x.Code == "VALIDATION_ERROR"))
                    statusCode = HttpStatusCode.BadRequest;
                else if (result.Errors.Any(x => x.Code == "UNAUTHORIZED_ACCESS"))
                    statusCode = HttpStatusCode.Forbidden;
            }

            return new ContentResult
            {
                Content = await _documentWriter.WriteToStringAsync(result),
                ContentType = "application/json; charset=utf-8",
                StatusCode = (int)statusCode
            };
        }
    }
}