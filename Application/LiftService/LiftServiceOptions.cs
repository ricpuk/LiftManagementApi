using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.LiftService
{
    public class LiftServiceOptions
    {
        public int FloorsMin { get; set; }
        public int FloorsMax { get; set; }
        public int Lifts { get; set; }
        public double LiftMovementTime { get; set; }
        public double DoorOpenCloseTime { get; set; }
    }
}
