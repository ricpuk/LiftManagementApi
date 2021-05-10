using System.Collections.Generic;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests
{
    public class LiftSchedulerTests
    {
        private ILiftScheduler _liftScheduler;
        private readonly Dictionary<int, List<int>> _operations = new();

        [SetUp]
        public void Setup()
        {
            var liftOperationRepository = new Mock<ILiftOperationRepository>(MockBehavior.Strict);
            liftOperationRepository.Setup(x => x.ScheduleOperation(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int liftId, int floor) => AddOperation(liftId, floor));
            liftOperationRepository.Setup(x => x.RemoveOperation(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int liftId, int floor) => RemoveOperation(liftId, floor));
            liftOperationRepository.Setup(x => x.GetOperations(It.IsAny<int>()))
                .Returns((int liftId) => _operations.TryGetValue(liftId, out var operations) ? operations : new List<int>());

            var floorSelectionStrategy = new Mock<IFloorSelectionStrategy>();

            _liftScheduler = new LiftScheduler.LiftScheduler(liftOperationRepository.Object, floorSelectionStrategy.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _liftScheduler = null;
            _operations.Clear();
        }

        [Test]
        public void ScheduleOperation_FloorProvided_NoExceptionThrown()
        {
            //Arrange
            var lift = new Lift(1, 0.1, 0.1, 1);
            //Act & Assert
            Assert.DoesNotThrow(() => _liftScheduler.ScheduleOperation(lift, 3));

        }

        [Test]
        public void ScheduleOperation_NoOperationsQueued_StateIdle()
        {
            //Arrange
            var lift = new Lift(1, 0.1, 0.1, 1);
            //Act & Assert
            _liftScheduler.ScheduleOperation(lift);

            Assert.AreEqual(LiftState.Idle, lift.State);

        }

        [Test]
        public void ScheduleOperation_OperationQueued_NoExceptionThrown()
        {
            //Arrange
            var lift = new Lift(1, 0.1, 0.1, 1);

            AddOperation(1, 4);

            //Act & Assert
            Assert.DoesNotThrow(() => _liftScheduler.ScheduleOperation(lift));
        }

        private bool AddOperation(int liftId, int floor)
        {
            if (!_operations.TryGetValue(liftId, out var operations))
            {
                _operations.Add(liftId, new List<int>{floor});
                return true;
            }

            if (!operations.Contains(floor))
            {
                operations.Add(floor);
            }

            return true;
        }

        private bool RemoveOperation(int liftId, int floor)
        {
            if (_operations.TryGetValue(liftId, out var operations))
            {
                operations.Remove(floor);
            }
            return true;
        }
    }
}
