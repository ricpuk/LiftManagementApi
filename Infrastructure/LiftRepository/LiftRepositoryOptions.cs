using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.LiftRepository
{
    public class LiftRepositoryOptions
    {
        public int Lifts { get; set; }
        public double LiftMovementTime { get; set; }
        public double DoorOpenCloseTime { get; set; }
    }
}
