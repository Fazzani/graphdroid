namespace POC_GraphQL.Models
{
    using GraphQL.DataLoader;
    using GraphQL.Relay.Types;
    using GraphQL.Types;
    using POC_GraphQL.Common;
    using POC_GraphQL.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Linq;
    using GraphQL.Validation;
    using GraphQL.Authorization;
    using global::Common;

    public class Droid : Character
    {
        public string PrimaryFunction { get; set; }
    }

    public class DroidGType : NodeGraphType<Droid>
    {
        IDroidRepository _droidRepository;

        public DroidGType(IDroidRepository droidRepository, IDataLoaderContextAccessor dataLoader)
        {
            _droidRepository = droidRepository;
            Connection<DroidGType>()
             .Name("droids")
             .Unidirectional()
             .PageSize(10)
             .ResolveAsync(async context =>
             {
                 var loader = dataLoader.Context.GetOrAddCollectionBatchLoader<Guid, Character>("FriendsLoader", _droidRepository.GetFriendsAsync);

                 // IMPORTANT: In order to avoid deadlocking on the loader we use the following construct (next 2 lines):
                 var loadHandle = loader.LoadAsync(context.Source.Id);
                 var result = await loadHandle;

                 return await result.ToConnection(context);
             });

            Name = "Droid";
            Description = "A mechanical creature in the Star Wars universe.";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("The unique identifier of the droid.");
            Field(x => x.Name).Description("The name of the droid.");
            Field(x => x.PrimaryFunction, nullable: true).Description("The primary function of the droid.");
            //Authorization on Field scope
            Field<ListGraphType<EpisodeGType>>(nameof(Droid.AppearsIn), "Which movie they appear in.").AuthorizeWith(Constants.Policies.AdminPolicy);

            FieldAsync<ListGraphType<CharacterInterface>, List<Character>>(
            nameof(Droid.Friends),
            "The friends of the character, or an empty list if they have none.",
            resolve: context =>
            {
              //  var userContext = context.UserContext as GraphQLUserContext;
              //  var authenticated = userContext.User?.Identity.IsAuthenticated ?? false;
              //  if (userContext!=null && userContext.User.Claims.Any(x => x.Value.Equals("Admin")))
              //  {
              //          context.ReportError(new ValidationError(
              //context.OriginalQuery,
              //"auth-required",
              //$"Authorization is required to access {op.Name}.",
              //op));
              //  }
                return droidRepository.GetFriendsAsync(context.Source, context.CancellationToken);
            });

            Interface<CharacterInterface>();
        }

        public override Droid GetById(string id)
        {
            return _droidRepository.GetAsync(id.FromCursor(), CancellationToken.None).Result;
        }
    }
}
