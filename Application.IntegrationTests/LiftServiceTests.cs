using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.LiftService;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.FloorSelection;
using Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Application.IntegrationTests
{
    public class LiftServiceTests
    {
        private ILiftService _liftService;
        private readonly int _numberOfLifts = 5;
        private readonly int _floorsMax = 5;
        private readonly int _floorsMin = 1;
        private readonly double _liftActionTime = 0.1;

        [SetUp]
        public void SetUp()
        {
            var options = Options.Create(new LiftServiceOptions
            {
                DoorOpenCloseTime = _liftActionTime,
                FloorsMax = _floorsMax,
                FloorsMin = _floorsMin,
                LiftMovementTime = _liftActionTime,
                Lifts = _numberOfLifts
            });

            var liftRepository = new InMemoryLiftRepository();

            var liftLogRepository = new InMemoryLiftLogRepository();

            var liftOperationRepository = new LiftOperationRepository();

            var floorSelection = new FifoFloorSelectionStrategy();

            var liftScheduler = new LiftScheduler.LiftScheduler(liftOperationRepository, floorSelection);

            _liftService = new LiftService.LiftService(
                liftRepository,
                liftLogRepository,
                options,
                liftScheduler);
        }

        [Test]
        public void Constructor_InitialState_InitCorrect()
        {
            Assert.AreEqual(_liftService.GetList().Count(), _numberOfLifts);
        }

        [TestCase(new [] {1, 2, 3, 5}, 5)]
        public void CallLift_MultipleCalls_FunctionalityCorrect(int[] operations, int timeout)
        {
            //Arrange
            var timeoutSpan = TimeSpan.FromSeconds(timeout);
            var expectedFloor = operations.Last();
            var liftId = 1;

            //Act
            foreach (var operation in operations)
            {
                _liftService.CallLift(liftId, new CallLiftDto {Floor = operation});
            }

            Thread.Sleep(timeoutSpan);

            //Assert
            var lift = _liftService.GetById(liftId);

            Assert.AreEqual(expectedFloor, lift.Floor);
            Assert.AreEqual(LiftState.Idle.ToString(), lift.LiftState);

        }

    }
}
