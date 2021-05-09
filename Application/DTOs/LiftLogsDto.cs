using System;
using Domain.Entities;

namespace Application.DTOs
{
    public class LiftLogDto
    {
        public LiftLogDto(LiftLog liftLog)
        {
            At = liftLog.At;
            Message = liftLog.Message;
        }

        public DateTime At { get; set; }
        public string Message { get; set; }
    }
}
