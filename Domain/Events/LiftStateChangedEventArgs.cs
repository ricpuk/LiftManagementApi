using System;

namespace Domain.Events
{
    public class LiftStateChangedEventArgs
    {
        public int LiftId { get; set; }
        public DateTime At { get; set; }
        public string Message { get; set; }
    }
}
