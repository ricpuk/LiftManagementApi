using System.Collections;
using System.Collections.Generic;
using Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface ILiftService
    {
        IEnumerable<LiftInfoDto> GetList();
        bool CallLift(int id, CallLiftDto request);
        LiftInfoDto GetById(int id);
        IEnumerable<LiftLogDto> GetLiftLogs(int lift);
    }
}
