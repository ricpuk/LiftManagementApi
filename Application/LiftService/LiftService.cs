using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace Application.LiftService
{
    public class LiftService : ILiftService
    {
        private readonly ILiftRepository _liftRepository;
        private readonly IOptionsMonitor<LiftServiceOptions> _options;

        public LiftService(ILiftRepository liftRepository)
        {
            _liftRepository = liftRepository;

        }

        public bool CallLift(CallLiftDto request)
        {
            throw new System.NotImplementedException();
        }

        public LiftLogsDto GetLiftLogs(GetLiftLogsDto lift)
        {
            throw new System.NotImplementedException();
        }
    }
}
