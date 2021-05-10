using Domain.Entities;

namespace Application.Interfaces
{
    /// <summary>
    /// Interface for lift scheduler.
    /// </summary>
    public interface ILiftScheduler
    {
        /// <summary>
        /// Schedule next operation after completion.
        /// </summary>
        /// <param name="lift">Lift to schedule operation for.</param>
        void ScheduleOperation(Lift lift);

        /// <summary>
        /// Schedule a new operation.
        /// </summary>
        /// <param name="lift">Lift to schedule operation for.</param>
        /// <param name="floor">Operation floor.</param>
        void ScheduleOperation(Lift lift, int floor);
    }
}
