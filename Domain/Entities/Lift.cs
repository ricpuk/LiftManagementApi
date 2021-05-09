using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class Lift
    {
        public Lift(int id, double secondsPerFloor, double doorActionTime)
        {
            Id = id;
            CurrentFloor = 0;
            _secondsPerFloor = TimeSpan.FromSeconds(secondsPerFloor);
            _doorActionTime = TimeSpan.FromSeconds(doorActionTime);
        }

        private readonly TimeSpan _secondsPerFloor;
        private readonly TimeSpan _doorActionTime;

        public int Id { get; set; }

        private LiftState _state;
        public LiftState State
        {
            get => _state;
            set
            {
                _state = value;
                Console.WriteLine($"Lift: {Id}, State: {_state}");
            }
        }

        public int CurrentFloor { get; private set; }

        public Task TravelTo(int floor)
        {
            if (floor == CurrentFloor)
            {
                return Task.CompletedTask;
            }

            if (CurrentFloor > floor)
            {
                State = LiftState.GoingDown;
                Task.Run(() => GoDown(floor));
            }
            else
            {
                State = LiftState.GoingUp;
                Task.Run(() => GoUp(floor));
            }

            return Task.CompletedTask;
        }

        private async Task GoDown(int floor)
        {
            while (CurrentFloor > floor)
            {
                Thread.Sleep(_secondsPerFloor);
                CurrentFloor -= 1;
            }
            await OpenAndCloseDoors();
        }

        private async Task GoUp(int floor)
        {
            while (floor > CurrentFloor)
            {
                Thread.Sleep(_secondsPerFloor);
                CurrentFloor += 1;
                Console.WriteLine($"Lift: {Id}, Floor: {CurrentFloor}");
            }

            await OpenAndCloseDoors();
        }

        private Task OpenAndCloseDoors()
        {
            State = LiftState.DoorsOpening;
            Thread.Sleep(_doorActionTime);
            State = LiftState.DoorsOpening;
            Thread.Sleep(_doorActionTime);
            State = LiftState.Idle;

            return Task.CompletedTask;
        }
    }
}
