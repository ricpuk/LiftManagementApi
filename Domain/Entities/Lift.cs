using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Events;

namespace Domain.Entities
{
    public class Lift
    {
        private readonly TimeSpan _secondsPerFloor;
        private readonly TimeSpan _doorActionTime;
        private readonly ConcurrentBag<LiftLog> _liftLogs = new();

        public Lift(int id, double secondsPerFloor, double doorActionTime, int startingFloor)
        {
            Id = id;
            _currentFloor = startingFloor;
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

        public EventHandler<LiftFinishedOperationEventArgs> OnActionCompleted { get; set; }
        public EventHandler<LiftStateChangedEventArgs> OnStateChanged { get; set; }

        public List<LiftLog> GetLiftLogs()
        {
            return _liftLogs.ToList();
        }

        public async Task TravelTo(int floor)
        {
                
            if (CurrentFloor > floor)
            {
                State = LiftState.GoingDown;
                await GoDown(floor);
            } 
            else
            {
                State = LiftState.GoingUp;
                await GoUp(floor);
            }

            await FinishOperation();
        }

        private Task GoDown(int floor)
        {
            while (CurrentFloor > floor)
            {
                Thread.Sleep(_secondsPerFloor);
                CurrentFloor -= 1;
            }

            return Task.CompletedTask;
        }

        private Task GoUp(int floor)
        {
            while (floor > CurrentFloor)
            {
                Thread.Sleep(_secondsPerFloor);
                CurrentFloor += 1;
            }

            return Task.CompletedTask;
        }

        private Task FinishOperation()
        {
            State = LiftState.DoorsOpening;
            Thread.Sleep(_doorActionTime);
            State = LiftState.DoorsClosing;
            Thread.Sleep(_doorActionTime);

            OnActionCompleted?.Invoke(this, new LiftFinishedOperationEventArgs
            {
                LiftId = Id
            });

            return Task.CompletedTask;
        }
    }
}
