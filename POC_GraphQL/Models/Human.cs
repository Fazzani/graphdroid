namespace POC_GraphQL.Models
{
    using System.Collections.Generic;
    using GraphQL.Types;
    using POC_GraphQL.Repositories;

    public class Human : Character
    {
        public string HomePlanet { get; set; }
    }

    public class HumanGType : ObjectGraphType<Human>
    {
        public HumanGType(IHumanRepository humanRepository)
        {
            Name = "Human";
            Description = "A humanoid creature from the Star Wars universe.";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("The unique identifier of the human.");
            Field(x => x.Name).Description("The name of the human.");
            Field(x => x.HomePlanet, nullable: true).Description("The home planet of the human.");
            Field<ListGraphType<EpisodeGType>>(nameof(Character.AppearsIn), "Which movie they appear in.");

            FieldAsync<ListGraphType<CharacterInterface>, List<Character>>(
                nameof(Human.Friends),
                "The friends of the character, or an empty list if they have none.",
                resolve: context => humanRepository.GetFriends(context.Source, context.CancellationToken));

            Interface<CharacterInterface>();
        }
    }
}
