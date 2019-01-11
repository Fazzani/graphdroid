namespace POC_GraphQL.ValidationRules
{
    using GraphQL.Language.AST;
    using GraphQL.Validation;
    using Microsoft.Extensions.Logging;

    public class DebugValidationRule : IValidationRule
    {
        ILogger _logger;
        public DebugValidationRule(ILogger<DebugValidationRule> logger)
        {
            _logger = logger;
        }
        public INodeVisitor Validate(ValidationContext context)
        {
            return new EnterLeaveListener(_ =>
            {
                _.Match<Operation>(enter => _logger.LogInformation($"NodeName: {enter.NameNode?.Name ?? ""} Operation Name: {enter.Name}"));
                _.Match<VariableDefinition>(enter => _logger.LogInformation($"NodeName: {enter.NameNode?.Name ?? ""} Variable Name: {enter.Name}"));
                _.Match<NamedType>(leave: node =>
                {
                    var type = context.Schema.FindType(node.Name);
                    _logger.LogInformation($"NodeName: {node.NameNode?.Name ?? ""} NamedType Name: {node.Name} Type {type.Name}");
                });
                _logger.LogInformation($"OriginalQuery => {context.Document.OriginalQuery}");
            });
        }
    }
}
