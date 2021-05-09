using System.Collections.Generic;
using System.Linq;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Options;

namespace Application.LiftService
{
    public class LiftService : ILiftService
    {
        private readonly ILiftRepository _liftRepository;
        private readonly LiftServiceOptions _options;

        public LiftService(ILiftRepository liftRepository, IOptions<LiftServiceOptions> options)
        {
            _liftRepository = liftRepository;
            _options = options.Value;
        }

        public IEnumerable<LiftListItemDto> GetList()
        {
            return _liftRepository.GetAll().Select(x => new LiftListItemDto(x)).ToList();
        }

        public bool CallLift(int id, CallLiftDto request)
        {
            if (request.Floor > _options.Floors || request.Floor < 0)
            {
                //Throw;
            }

            var lift = _liftRepository.GetById(id);
            if (lift == null)
            {
                return false; // TODO throw
            }

            lift.TravelTo(request.Floor);

            return true;
            
        }

        public LiftLogsDto GetLiftLogs(GetLiftLogsDto lift)
        {
            throw new System.NotImplementedException();
        }
    }
}
