namespace POC_GraphQL.Models
{
    using GraphQL.Types;

    public class HumanInputObject : InputObjectGraphType
    {
        public HumanInputObject()
        {
            Name = "HumanInput";
            Description = "A humanoid creature from the Star Wars universe.";

            Field<NonNullGraphType<StringGraphType>>(nameof(Human.Name));
            Field<StringGraphType>(nameof(Human.HomePlanet));
            Field<ListGraphType<EpisodeGType>>(nameof(Human.AppearsIn), "Which movie they appear in.");
        }
    }
}
