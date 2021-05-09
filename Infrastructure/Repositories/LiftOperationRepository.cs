using System.Collections.Generic;
using Application.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// In memory implementation of lift operation repository.
    /// </summary>
    public class LiftOperationRepository : ILiftOperationRepository
    {
        private readonly Dictionary<int, List<int>> _operations = new();
        private readonly object _lock = new();

        /// <inheritdoc />
        public IEnumerable<int> GetOperations(int liftId)
        {
            lock (_lock)
            {
                if (_operations.TryGetValue(liftId, out var operations))
                {
                    return operations;
                }
            }

            return new List<int>();
        }

        /// <inheritdoc />
        public bool ScheduleOperation(int liftId, int floor)
        {
            lock (_lock)
            {
                if (_operations.TryGetValue(liftId, out var operations))
                {
                    if (operations.Contains(floor))
                    {
                        return true;
                    }
                    operations.Add(floor);
                }
                else
                {
                    _operations.Add(liftId, new List<int> { floor });
                }

            }

            return true;
        }

        /// <inheritdoc />
        public bool RemoveOperation(int liftId, int floor)
        {
            lock (_lock)
            {
                if (!_operations.TryGetValue(liftId, out var operations))
                {
                    return false;
                }

                operations.Remove(floor);

            }

            return true;

        }
    }
}
