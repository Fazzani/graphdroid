namespace POC_GraphQL.Repositories
{
    using POC_GraphQL.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDroidRepository
    {
        Task<Droid> GetDroid(Guid id, CancellationToken cancellationToken);

        Task<List<Character>> GetFriends(Droid droid, CancellationToken cancellationToken);
    }
}
