using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface ILiftOperationRepository
    {
        IEnumerable<int> GetOperations(int liftId);
        bool ScheduleOperation(int liftId, int floor);
        bool RemoveOperation(int liftId, int floor);
    }
}
