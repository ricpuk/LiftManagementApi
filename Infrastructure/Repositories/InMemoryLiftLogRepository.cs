using System.Collections.Generic;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class InMemoryLiftLogRepository : ILiftLogRepository
    {
        private readonly Dictionary<int, List<LiftLog>> _logs = new ();
        private readonly object _lock = new();

        public void Log(int liftId, LiftLog log)
        {
            lock (_lock)
            {
                var exist = _logs.TryGetValue(liftId, out var logs);
                if (exist)
                {
                    logs.Add(log);
                }
                else
                {
                    _logs.Add(liftId, new List<LiftLog>{log});
                }
            }
        }

        public IEnumerable<LiftLog> GetLogs(int liftId)
        {
            lock (_lock)
            {
                if (_logs.TryGetValue(liftId, out var logs))
                {
                    return logs;
                }

                return new List<LiftLog>();
            }
        }
    }
}
