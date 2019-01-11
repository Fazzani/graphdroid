namespace POC_GraphQL.ValidationRules
{
    using GraphQL.Validation;
    using System;

    public class DebugValidationRule : IValidationRule
    {
        public INodeVisitor Validate(ValidationContext context)
        {
            return new EnterLeaveListener(_ =>
            {
                Console.WriteLine($"OriginalQuery => {context.Document.OriginalQuery}");
            });
        }
    }
}
