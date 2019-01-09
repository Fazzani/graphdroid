namespace POC_GraphQL.Repositories
{
    using POC_GraphQL.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class DroidRepository : IDroidRepository
    {
        public Task<List<Droid>> GetAllAsync(CancellationToken cancellationToken) =>
            Task.FromResult(Database.Droids);

        public Task<Droid> GetAsync(Guid id, CancellationToken cancellationToken) =>
            Task.FromResult(Database.Droids.FirstOrDefault(x => x.Id == id));

        public Task<List<Character>> GetFriendsAsync(Droid droid, CancellationToken cancellationToken) =>
            Task.FromResult(Database.Characters.Where(x => droid.Friends.Contains(x.Id)).ToList());


        public Task<ILookup<Guid, Character>> GetFriendsAsync(IEnumerable<Guid> humansId, CancellationToken cancellationToken) =>
            Task.FromResult(Database.Characters.Where(x => humansId.Contains(x.Id)).ToLookup(d => d.Id));

    }
}