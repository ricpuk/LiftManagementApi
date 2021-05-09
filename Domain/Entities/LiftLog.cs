using System;

namespace Domain.Entities
{
    public class LiftLog
    {
        public LiftLog(DateTime time, string message)
        {
            At = time;
            Message = message;
        }
        public DateTime At { get; set; }
        public string Message { get; set; }

        //Could add additional properties like event type etc.
    }
}
