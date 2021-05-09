using System.Collections.Concurrent;
using System.Collections.Generic;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    class InMemoryLiftRepository : ILiftRepository
    {
        private readonly ConcurrentDictionary<int, Lift> _lifts = new();
        public IEnumerable<Lift> GetAll()
        {
            return _lifts.Values;
        }

        public Lift GetById(int id)
        {
            return !_lifts.TryGetValue(id, out var lift) ? null : lift;
        } 

        public bool Add(Lift lift)
        {
            return _lifts.TryAdd(lift.Id, lift);
        }
    }
}
