using System.Collections;
using System.Collections.Generic;
using Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface ILiftService
    {
        IEnumerable<LiftListItemDto> GetList();
        bool CallLift(int id, CallLiftDto request);
        LiftLogsDto GetLiftLogs(GetLiftLogsDto lift);
    }
}
