using POC_GraphQL.Repositories;
namespace POC_GraphQL.Models
{
    public class HumanCreatedEvent : HumanType
    {
        public HumanCreatedEvent(IHumanRepository humanRepository) : base(humanRepository)
        {
        }
    }
}
