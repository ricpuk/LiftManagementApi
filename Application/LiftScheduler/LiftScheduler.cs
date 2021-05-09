﻿using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;

namespace Application.LiftScheduler
{
    public class LiftScheduler : ILiftScheduler
    {
        private readonly ILiftOperationRepository _liftOperationRepository;
        private readonly IFloorSelectionStrategy _floorSelection;

        public LiftScheduler(ILiftOperationRepository liftOperationRepository, IFloorSelectionStrategy floorSelection)
        {
            _liftOperationRepository = liftOperationRepository;
            _floorSelection = floorSelection;
        }

        public void SchedlueOperation(Lift lift)
        {
            var liftId = lift.Id;
            _liftOperationRepository.RemoveOperation(liftId, lift.CurrentFloor);
            var requests = _liftOperationRepository.GetOperations(liftId).ToList();

            if (requests.Any())
            {
                var floor = _floorSelection.SelectFloor(lift.CurrentFloor, requests);
                Schedule(lift, floor);
            }
            else
            {
                lift.State = LiftState.Idle;
            }
        }

        public void SchedlueOperation(Lift lift, int floor)
        {
            var liftId = lift.Id;
            _liftOperationRepository.ScheduleOperation(liftId, floor);

            if (lift.State == LiftState.Idle)
            {
                Schedule(lift, floor);
            }
        }

        private Task Schedule(Lift lift, int floor)
        {
            return Task.Run(() => lift.TravelTo(floor));
        }
    }
}