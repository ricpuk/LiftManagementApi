using System.Collections.Generic;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ILiftLogRepository
    {
        void Log(int litId, LiftLog log);

        IEnumerable<LiftLog> GetLogs(int liftId);
    }
}
