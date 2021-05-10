using System;
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

        private readonly object _lock = new();

        public Lift(int id, double secondsPerFloor, double doorActionTime, int startingFloor)
        {
            Id = id;
            _currentFloor = startingFloor;
            _secondsPerFloor = TimeSpan.FromSeconds(secondsPerFloor);
            _doorActionTime = TimeSpan.FromSeconds(doorActionTime);
        }

        

        public int Id { get; }

        private LiftState _state;
        public LiftState State
        {
            get
            {
                lock (_lock)
                {
                    return _state;
                }
            }
            set 
            {
                lock (_lock)
                {
                    NotifyHandlers($"Lift state change: {_state} -> {value}");
                    _state = value;
                }
                
            }
        }

        private int _currentFloor;
        public int CurrentFloor
        {
            get
            {
                lock (_lock)
                {
                    return _currentFloor;
                }
            }
            private set
            {
                lock (_lock)
                {
                    NotifyHandlers($"Lift floor change: {_currentFloor} -> {value}");
                    _currentFloor = value;
                }
            }
        }

        public EventHandler<LiftFinishedOperationEventArgs> OnActionCompleted { get; set; }
        public EventHandler<LiftStateChangedEventArgs> OnStateChanged { get; set; }

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

        public bool IsIdle()
        {
            return State == LiftState.Idle;
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

        private void NotifyHandlers(string message)
        {
            OnStateChanged?.Invoke(this, new LiftStateChangedEventArgs
            {
                At = DateTime.UtcNow,
                LiftId = Id,
                Message = message
            });
        }
    }
}
