using System.Collections.Generic;

namespace Application.Interfaces
{
    /// <summary>
    /// Interface for floor lift floor selection strategy.
    /// </summary>
    public interface IFloorSelectionStrategy
    {
        /// <summary>
        /// Selects next floor according to current floor and lift calls.
        /// </summary>
        /// <param name="currentFloor">Current floor.</param>
        /// <param name="floors">Lift floor requests.</param>
        /// <returns></returns>
        int SelectFloor(int currentFloor, IEnumerable<int> floors);
    }
}
