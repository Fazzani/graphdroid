namespace POC_GraphQL.Repositories
{
    using POC_GraphQL.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDroidRepository
    {
        Task<Droid> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Droid>> GetAllAsync( CancellationToken cancellationToken);
        Task<List<Character>> GetFriendsAsync(Droid droid, CancellationToken cancellationToken);
        Task<ILookup<Guid, Character>> GetFriendsAsync(IEnumerable<Guid> humansId, CancellationToken cancellationToken);
    }
}
