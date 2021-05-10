using System;
using System.Linq;
using System.Threading;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.FloorSelection;
using Infrastructure.Repositories;
using NUnit.Framework;

namespace Application.IntegrationTests
{
    public class LiftManagerTests
    {
        private ILiftScheduler _liftScheduler;
        private Lift _lift = new Lift(1, 0.1, 0.1, 1);

        [SetUp]
        public void SetUp()
        {
            ILiftOperationRepository operationRepository = new LiftOperationRepository();
            IFloorSelectionStrategy floorSelection = new FifoFloorSelectionStrategy();
            _liftScheduler = new LiftScheduler.LiftScheduler(operationRepository, floorSelection);
        }

        [TestCase(5, 1)]
        public void ScheduleOperation_FloorProvided_TravelledCorrectly(int floor, double timeout)
        {
            //Arrange
            var expectedFloor = floor;
            var timeoutSpan = TimeSpan.FromSeconds(timeout);
            //Act
            _liftScheduler.ScheduleOperation(_lift, floor);
            Thread.Sleep(timeoutSpan);
            //Assert
            Assert.AreEqual(expectedFloor, _lift.CurrentFloor);
        }


        [TearDown]
        public void TearDown()
        {
            _liftScheduler = null;
        }
    }
}
