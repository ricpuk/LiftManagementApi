using Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface ILiftService
    {
        bool CallLift(CallLiftDto request);
        LiftLogsDto GetLiftLogs(GetLiftLogsDto lift);
    }
}
