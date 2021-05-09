using System.Collections.Concurrent;
using System.Collections.Generic;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace Infrastructure.LiftRepository
{
    class InMemoryLiftRepository : ILiftRepository
    {
        public InMemoryLiftRepository(IOptions<LiftRepositoryOptions> options)
        {
            var optionsValue = options.Value;
            for (int i = 1; i <= optionsValue.Lifts; i++)
            {
                var lift = new Lift(i, optionsValue.LiftMovementTime, optionsValue.DoorOpenCloseTime);
                _lifts.TryAdd(lift.Id, lift);
            }
        }

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
