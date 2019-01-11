namespace POC_GraphQL.Queries
{
    using GraphQL.Relay.Types;
    using GraphQL.Types;
    using POC_GraphQL.Common;
    using POC_GraphQL.Models;
    using POC_GraphQL.Repositories;
    using System;
    using System.Linq;
    using System.Linq.Dynamic.Core;

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
    public class RootQuery : QueryGraphType
    {
        public RootQuery(
            IDroidRepository droidRepository,
            IHumanRepository humanRepository)
        {

            Connection<HumanGType>()
               .Name("humans")
               .Argument<StringGraphType>("filter", "Filter humans")
               .Unidirectional()
               .PageSize(10)
               .Resolve(context =>
               {
                   var filter = context.GetArgument<string>("filter");
                   Console.WriteLine($"filter => {filter}");
                   if (filter == null)
                   {
                       return humanRepository.GetAll(context.CancellationToken).Result.ToConnection(context);
                       //return (await humanRepository.GetAll(context.CancellationToken)).ToConnection(context);
                   }
                   else
                   {
                       return humanRepository.GetAll(context.CancellationToken).Result.AsQueryable().Where(filter).ToConnection(context);
                       //return (await humanRepository.GetAll(context.CancellationToken)).AsQueryable().Where(filter).ToConnection(context);
                   }
               });

            Connection<DroidGType>()
              .Name("droids")
              .Argument<StringGraphType>("filter", "Filter droids")
              .Unidirectional()
              .PageSize(10)
              .ResolveAsync(async context =>
              {
                  var filter = context.GetArgument<string>("filter");
                  Console.WriteLine($"filter => {filter}");
                  if (filter == null)
                  {
                      var result = await droidRepository.GetAllAsync(context.CancellationToken);
                      return result.ToConnection(context);
                  }
                  else
                  {
                      var result = await droidRepository.GetAllAsync(context.CancellationToken);
                      return result.AsQueryable().Where(filter).ToConnection(context);
                  }
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
