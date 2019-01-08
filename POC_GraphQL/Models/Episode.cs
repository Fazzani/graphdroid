namespace POC_GraphQL.Models
{
    using GraphQL.Types;
    public enum Episode
    {
        NEWHOPE = 4,
        EMPIRE = 5,
        JEDI = 6
    }

    public class EpisodeGType : EnumerationGraphType<Episode>
    {
        public EpisodeGType()
        {
            Name = "Episode";
            Description = "One of the films in the Star Wars Trilogy.";

            AddValue("NEWHOPE", "Released in 1977.", 4);
            AddValue("EMPIRE", "Released in 1980.", 5);
            AddValue("JEDI", "Released in 1983.", 6);
        }
    }
}
