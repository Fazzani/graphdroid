namespace POC_GraphQL.Models
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using GraphQL.DataLoader;
    using GraphQL.Relay.Types;
    using GraphQL.Types;
    using POC_GraphQL.Common;
    using POC_GraphQL.Repositories;

    public class Human : Character
    {
        public string HomePlanet { get; set; }
    }

    public class HumanGType : NodeGraphType<Human>
    {
        IHumanRepository _humanRepository;

        public HumanGType(IHumanRepository humanRepository, IDataLoaderContextAccessor dataLoader)
        {
            _humanRepository = humanRepository;

            Connection<HumanGType>()
                .Name("humans")
                .Unidirectional()
                .PageSize(10)
                .ResolveAsync(async context =>
                {
                    var loader = dataLoader.Context.GetOrAddCollectionBatchLoader<Guid, Character>("FreindsLoader", _humanRepository.GetFriendsAsync);

                    // IMPORTANT: In order to avoid deadlocking on the loader we use the following construct (next 2 lines):
                    var loadHandle = loader.LoadAsync(context.Source.Id);
                    var result = await loadHandle;

                    return await result.ToConnection(context);
                });


            Name = "Human";
            Description = "A humanoid creature from the Star Wars universe.";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("The unique identifier of the human.");
            Field(x => x.Name).Description("The name of the human.");
            Field(x => x.HomePlanet, nullable: true).Description("The home planet of the human.");
            Field<ListGraphType<EpisodeGType>>(nameof(Character.AppearsIn), "Which movie they appear in.");

            FieldAsync<ListGraphType<CharacterInterface>, List<Character>>(
                nameof(Human.Friends),
                "The friends of the character, or an empty list if they have none.",
                resolve: context => _humanRepository.GetFriendsAsync(context.Source, context.CancellationToken));

            Interface<CharacterInterface>();
        }
        
        public override Human GetById(string id)
        {
            return _humanRepository.GetAsync(id.FromCursor(), CancellationToken.None).Result;
        }
    }
}
