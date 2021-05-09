using System.Collections;
using System.Collections.Generic;
using Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface ILiftService
    {
        IEnumerable<LiftInfoDto> GetList();
        bool CallLift(int id, CallLiftDto request);
        List<LiftLogDto> GetLiftLogs(int lift);
    }
}
