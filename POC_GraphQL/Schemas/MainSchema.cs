namespace POC_GraphQL.Schemas
{
    using GraphQL;
    using GraphQL.Types;
    using POC_GraphQL.Queries;

    public class MainSchema : Schema
    {
        public MainSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            this.Query = resolver.Resolve<RootQuery>();
            this.Mutation = resolver.Resolve<RootMutation>();
            this.Subscription = resolver.Resolve<RootSubscription>();
        }
    }
}
