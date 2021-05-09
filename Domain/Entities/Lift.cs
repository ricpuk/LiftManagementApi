using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class Lift
    {
        private readonly TimeSpan _secondsPerFloor;
        private readonly TimeSpan _doorActionTime;
        private readonly ConcurrentBag<LiftLog> _liftLogs = new();

        public Lift(int id, double secondsPerFloor, double doorActionTime)
        {
            Id = id;
            _currentFloor = 1;
            _secondsPerFloor = TimeSpan.FromSeconds(secondsPerFloor);
            _doorActionTime = TimeSpan.FromSeconds(doorActionTime);
        }

        

        public int Id { get; set; }

        private LiftState _state;
        public LiftState State
        {
            get => _state;
            set
            {
                _liftLogs.Add(new LiftLog(DateTime.UtcNow, $"Lift state change: {_state} -> {value}"));
                _state = value;
            }
        }

        private int _currentFloor;
        public int CurrentFloor
        {
            get => _currentFloor;
            private set
            {

                _liftLogs.Add(new LiftLog(DateTime.UtcNow, $"Lift floor change: {_currentFloor} -> {value}"));
                _currentFloor = value;
            }
        }

        public List<LiftLog> GetLiftLogs()
        {
            return _liftLogs.ToList();
        }

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
            }

            await OpenAndCloseDoors();
        }

        private Task OpenAndCloseDoors()
        {
            State = LiftState.DoorsOpening;
            Thread.Sleep(_doorActionTime);
            State = LiftState.DoorsClosing;
            Thread.Sleep(_doorActionTime);
            State = LiftState.Idle;

            return Task.CompletedTask;
        }
    }
}
