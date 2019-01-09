namespace POC_GraphQL.Repositories
{
    using POC_GraphQL.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Tasks;

    public class HumanRepository : IHumanRepository
    {
        private readonly Subject<Human> whenHumanCreated;

        public HumanRepository() => this.whenHumanCreated = new Subject<Human>();

        public IObservable<Human> WhenHumanCreated => this.whenHumanCreated.AsObservable();

        public Task<Human> AddAsync(Human human, CancellationToken cancellationToken)
        {
            human.Id = Guid.NewGuid();
            Console.WriteLine($"Creating new Human {human.Id}");
            Database.Humans.Add(human);
            whenHumanCreated.OnNext(human);
            return Task.FromResult(human);
        }

        public Task<ILookup<Guid,Character>> GetFriendsAsync(IEnumerable<Guid> humansId, CancellationToken cancellationToken) =>
            Task.FromResult(Database.Characters.Where(x => humansId.Contains(x.Id)).ToLookup(d=>d.Id));

        public Task<List<Character>> GetFriendsAsync(Human human, CancellationToken cancellationToken) =>
           Task.FromResult(Database.Characters.Where(x => human.Friends != null && human.Friends.Contains(x.Id)).ToList());

        public Task<Human> GetAsync(Guid id, CancellationToken cancellationToken) =>
            Task.FromResult(Database.Humans.FirstOrDefault(x => x.Id == id));

        public Task<List<Human>> GetAll(CancellationToken cancellationToken) =>
            Task.FromResult(Database.Humans);
    }
}
