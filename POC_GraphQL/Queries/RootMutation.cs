namespace POC_GraphQL.Queries
{
    using GraphQL.Types;
    using POC_GraphQL.Models;
    using POC_GraphQL.Repositories;

    /// <example>
    /// This is an example mutation to create a new human:
    /// mutation createHuman($human: HumanInput!) {
    ///   createHuman(human: $human)
    ///   {
    ///     id,
    ///     name
    ///   }
    /// }
    /// This is an example JSON of the variables you also need to specify to create a new human:
    /// {
    ///  "human": {
    ///     "name": "Muhammad Rehan Saeed",
    ///     "homePlanet": "Earth"
    ///   }
    /// }
    /// </example>
    public class RootMutation : ObjectGraphType<object>
    {
        public RootMutation(IHumanRepository humanRepository)
        {
            Name = "Mutation";
            Description = "The mutation type, represents all updates we can make to our data.";

            FieldAsync<HumanGType, Human>(
                "createHuman",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<HumanInputObject>>()
                    {
                        Name = "human",
                        Description = "The human you want to create."
                    }),
                resolve: context =>
                {
                    var human = context.GetArgument<Human>("human");
                    return humanRepository.AddAsync(human, context.CancellationToken);
                });
        }
    }
}
