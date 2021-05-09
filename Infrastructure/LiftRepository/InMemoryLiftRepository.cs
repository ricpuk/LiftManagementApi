using System.Collections.Concurrent;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.LiftRepository
{
    class InMemoryLiftRepository : ILiftRepository
    {
        private readonly ConcurrentDictionary<int, Lift> _lifts = new();
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
