using GraphQL.DataLoader;
using POC_GraphQL.Repositories;
namespace POC_GraphQL.Models
{
    public class HumanCreatedEvent : HumanGType
    {
        public HumanCreatedEvent(IHumanRepository humanRepository, IDataLoaderContextAccessor dataLoader) : 
            base(humanRepository, dataLoader)
        {
        }
    }
}
