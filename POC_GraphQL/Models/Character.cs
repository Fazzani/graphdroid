namespace POC_GraphQL.Models
{
    using GraphQL.Types;
    using POC_GraphQL.Common;
    using System;
    using System.Collections.Generic;

    public abstract class Character : IId
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Guid> Friends { get; set; }
        public List<Episode> AppearsIn { get; set; }
    }

    public class CharacterInterface : InterfaceGraphType<Character>
    {
        public CharacterInterface()
        {
            Name = "Character";
            Description = "A character from the Star Wars universe";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("The unique identifier of the character.");
            Field(x => x.Name, nullable: true).Description("The name of the character.");
            Field<ListGraphType<EpisodeGType>>(nameof(Character.AppearsIn), "Which movie they appear in.");

            Field<ListGraphType<CharacterInterface>>(
                nameof(Character.Friends),
                "The friends of the character, or an empty list if they have none."
                );
        }
    }
}
