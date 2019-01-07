namespace POC_GraphQL.Models
{
    using GraphQL.Types;
    using POC_GraphQL.Repositories;
    using System.Collections.Generic;

    public class Droid : Character
    {
        public string PrimaryFunction { get; set; }
    }

    public class DroidType : ObjectGraphType<Droid>
    {
        public DroidType(IDroidRepository droidRepository)
        {
            Name = "Droid";
            Description = "A mechanical creature in the Star Wars universe.";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("The unique identifier of the droid.");
            Field(x => x.Name).Description("The name of the droid.");
            Field(x => x.PrimaryFunction, nullable: true).Description("The primary function of the droid.");
            Field<ListGraphType<EpisodeType>>(nameof(Droid.AppearsIn), "Which movie they appear in.");

            FieldAsync<ListGraphType<CharacterInterface>, List<Character>>(
            nameof(Droid.Friends),
            "The friends of the character, or an empty list if they have none.",
            resolve: context => droidRepository.GetFriends(context.Source, context.CancellationToken));

            Interface<CharacterInterface>();
        }
    }
}
