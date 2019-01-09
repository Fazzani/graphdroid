namespace POC_GraphQL.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using GraphQL.Types;
    using POC_GraphQL.Models;
    using POC_GraphQL.Repositories;
    using POC_GraphQL.Common;

    /// <example>
    /// The is an example query to get a human and the details of their friends:
    /// query getHuman {
    ///   human(id: "94fbd693-2027-4804-bf40-ed427fe76fda")
    ///   {
    ///     id,
    ///     name,
    ///     homePlanet,
    ///     appearsIn,
    ///     friends {
    ///       name
    ///       ... on Droid {
    ///         primaryFunction
    ///       }
    ///   	  ... on Human
    ///       {
    ///         homePlanet
    ///       }
    ///     }
    ///   }
    /// }
    /// </example>
    public class RootQuery : ObjectGraphType<object>
    {
        public RootQuery(
            IDroidRepository droidRepository,
            IHumanRepository humanRepository)
        {

            Connection<HumanGType>()
               .Name("humans")
               .Unidirectional()
               .PageSize(10)
               .ResolveAsync(async context =>
               {
                   return await humanRepository.GetAll(CancellationToken.None).Result.ToConnection(context);
               });

            Connection<DroidGType>()
              .Name("droids")
              .Unidirectional()
              .PageSize(10)
              .ResolveAsync(async context =>
              {
                  return await droidRepository.GetAllAsync(CancellationToken.None).Result.ToConnection(context);
              });

            //this.Name = "Query";
            //this.Description = "The query type, represents all of the entry points into our object graph.";

            //this.FieldAsync<DroidGType, Droid>(
            //    "droid",
            //    arguments: new QueryArguments(
            //        new QueryArgument<IdGraphType>
            //        {
            //            Name = "id",
            //            Description = "The unique identifier of the droid.",
            //        }),
            //    resolve: context =>
            //        droidRepository.GetAsync(
            //            context.GetArgument("id", defaultValue: new Guid("1ae34c3b-c1a0-4b7b-9375-c5a221d49e68")),
            //            context.CancellationToken));
            //this.FieldAsync<HumanGType, Human>(
            //    "human",
            //    arguments: new QueryArguments(
            //        new QueryArgument<IdGraphType>()
            //        {
            //            Name = "id",
            //            Description = "The unique identifier of the human.",
            //        }),
            //    resolve: context => humanRepository.GetAsync(
            //        context.GetArgument("id", defaultValue: new Guid("94fbd693-2027-4804-bf40-ed427fe76fda")),
            //        context.CancellationToken));

            //this.FieldAsync<ListGraphType<HumanGType>, List<Human>>(
            //    "humans",                
            //    resolve: context => humanRepository.GetHumans(context.CancellationToken));
        }
    }
}
