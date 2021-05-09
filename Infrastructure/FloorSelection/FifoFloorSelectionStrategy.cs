using System.Collections.Generic;
using System.Linq;
using Application.Interfaces;

namespace Infrastructure.FloorSelection
{
    /// <summary>
    /// Fifo implementation of floor selection strategy.
    /// </summary>
    public class FifoFloorSelectionStrategy : IFloorSelectionStrategy
    {
        /// <inheritdoc />
        public int SelectFloor(int currentFloor, IEnumerable<int> floors)
        {
            //currentFloor is not needed in fifo case.
            return floors.First();
        }
    }
}
