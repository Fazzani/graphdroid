namespace POC_GraphQL.Controllers
{
    using GraphQL;
    using GraphQL.Types;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using POC_GraphQL.Queries;
    using System;
    using System.Threading.Tasks;

    [Route("[controller]")]
    public class GraphQLController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;
        readonly ILogger<GraphQLController> _logger;

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter, ILogger<GraphQLController> logger)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs
            };

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

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}