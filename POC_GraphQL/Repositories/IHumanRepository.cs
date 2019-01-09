namespace POC_GraphQL.Repositories
{
    using POC_GraphQL.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IHumanRepository
    {
        IObservable<Human> WhenHumanCreated { get; }

        Task<Human> AddAsync(Human human, CancellationToken cancellationToken);

        Task<ILookup<Guid, Character>> GetFriendsAsync(IEnumerable<Guid> humansId, CancellationToken cancellationToken);

        Task<List<Character>> GetFriendsAsync(Human human, CancellationToken cancellationToken);

        Task<Human> GetAsync(Guid id, CancellationToken cancellationToken);

        Task<List<Human>> GetAll(CancellationToken cancellationToken);
    }
}
