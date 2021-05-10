using System.Linq;
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

        public void ScheduleOperation(Lift lift)
        {
            var liftId = lift.Id;
            _liftOperationRepository.RemoveOperation(liftId, lift.CurrentFloor);
            var requests = _liftOperationRepository.GetOperations(liftId).ToList();
            // Additional optimizations can be made here.
            // I.e check for any other requests that the lift can handle on its way to defined one.
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

        public void ScheduleOperation(Lift lift, int floor)
        {
            var liftId = lift.Id;
            var liftOperations = _liftOperationRepository.GetOperations(liftId);
            _liftOperationRepository.ScheduleOperation(liftId, floor);

            if (lift.IsIdle() && !liftOperations.Any())
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
