using System;
using System.Collections.Generic;
using System.Linq;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Events;
using Microsoft.Extensions.Options;

namespace Application.LiftService
{
    public class LiftService : ILiftService
    {
        private readonly ILiftRepository _liftRepository;
        private readonly ILiftLogRepository _liftLogRepository;
        private readonly ILiftScheduler _liftScheduler;
        private readonly LiftServiceOptions _options;

        public LiftService(ILiftRepository liftRepository, ILiftLogRepository liftLogRepository, IOptions<LiftServiceOptions> options, ILiftScheduler liftScheduler)
        {
            _liftRepository = liftRepository;
            _liftLogRepository = liftLogRepository;
            _liftScheduler = liftScheduler;
            _options = options.Value;

            for (int i = 1; i <= _options.Lifts; i++)
            {
                var lift = new Lift(i, _options.LiftMovementTime, _options.DoorOpenCloseTime, _options.FloorsMin);
                lift.OnActionCompleted += OnLiftFinishedOperation;
                lift.OnStateChanged += OnLiftStateChange;
                _liftRepository.Add(lift);
            }

        }

        public IEnumerable<LiftInfoDto> GetList()
        {
            return _liftRepository.GetAll().Select(x => new LiftInfoDto(x)).ToList();
        }

        public LiftInfoDto GetById(int id)
        {
            var lift = _liftRepository.GetById(id);
            if (lift == null)
            {
                throw new NotFoundException($"Lift with id: {id} does not exist.");
            }
            return new LiftInfoDto(lift);
        }

        public bool CallLift(int id, CallLiftDto request)
        {
            if (request.Floor > _options.FloorsMax || request.Floor < _options.FloorsMin)
            {
                return false;
            }

            var lift = _liftRepository.GetById(id);
            RecordNewCall(id, request.Floor);
            _liftScheduler.ScheduleOperation(lift, request.Floor);
            
            return true;
            
        }

        public IEnumerable<LiftLogDto> GetLiftLogs(int id)
        {
            var logs = _liftLogRepository.GetLogs(id);
            return logs.Select(x => new LiftLogDto(x)).ToList();
        }

        private void OnLiftFinishedOperation(object sender, LiftFinishedOperationEventArgs eventArgs)
        {
            var liftId = eventArgs.LiftId;
            var lift = _liftRepository.GetById(liftId);
            _liftScheduler.ScheduleOperation(lift);
            
        }

        private void OnLiftStateChange(object sender, LiftStateChangedEventArgs eventArgs)
        {
            var liftId = eventArgs.LiftId;
            var date = eventArgs.At;
            var message = eventArgs.Message;
            var liftLog = new LiftLog(date, message);
            _liftLogRepository.Log(liftId, liftLog);
        }

        private void RecordNewCall(int id, int floor)
        {
            var log = new LiftLog(DateTime.UtcNow, $"Lift called to floor {floor}");
            _liftLogRepository.Log(id, log);
        }
    }
}
