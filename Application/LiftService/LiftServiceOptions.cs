using System.ComponentModel.DataAnnotations;

namespace Application.LiftService
{
    public class LiftServiceOptions
    {
        public int FloorsMin { get; set; }
        public int FloorsMax { get; set; }
        [Range(2, int.MaxValue, ErrorMessage = "Please configure atleast 2 lifts.")]
        public int Lifts { get; set; }
        public double LiftMovementTime { get; set; }
        public double DoorOpenCloseTime { get; set; }
    }
}
