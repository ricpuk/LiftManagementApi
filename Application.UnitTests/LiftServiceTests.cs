using System.Collections.Generic;
using System.Linq;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.LiftService;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests
{
    public class LiftServiceTests
    {
        private ILiftService _liftService;
        private readonly int _numberOfLifts = 5;
        private readonly int _floorsMax = 5;
        private readonly int _floorsMin = 1;
        private readonly double _liftActionTime = 0.1;
        private readonly Dictionary<int, Lift> _lifts = new();

        [SetUp]
        public void Setup()
        {
            var options = Options.Create(new LiftServiceOptions
            {
                DoorOpenCloseTime = _liftActionTime,
                FloorsMax = _floorsMax,
                FloorsMin = _floorsMin,
                LiftMovementTime = _liftActionTime,
                Lifts = _numberOfLifts
            });

            var liftRepositoryMock = new Mock<ILiftRepository>(MockBehavior.Strict);
            liftRepositoryMock.Setup(x => x.Add(It.IsAny<Lift>())).Returns(true);
            liftRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns<Lift>(null);
            for (int i = 1; i <= options.Value.Lifts; i++)
            {
                var lift = new Lift(i, _liftActionTime, _liftActionTime, _floorsMin);
                _lifts.Add(i, lift);
                liftRepositoryMock.Setup(x => x.GetById(lift.Id)).Returns(lift);
            }

            liftRepositoryMock.Setup(x => x.GetAll()).Returns(_lifts.Values);

            var liftLogRepositoryMock = new Mock<ILiftLogRepository>();

            var liftSchedulerMock = new Mock<ILiftScheduler>(MockBehavior.Strict);
            liftSchedulerMock.Setup(x => x.SchedlueOperation(It.IsAny<Lift>(), It.IsAny<int>()));

            _liftService = new LiftService.LiftService(
                liftRepositoryMock.Object, 
                liftLogRepositoryMock.Object, 
                options, 
                liftSchedulerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _liftService = null;
            _lifts.Clear();
        }

        [Test]
        public void GetAll_InitialState_CorrectAmountOfLifts()
        {
            //Arrange
            var expected = _numberOfLifts;

            //Act
            var actual = _liftService.GetList();

            //Assert
            Assert.AreEqual(expected, actual.Count());
        }

        [Test]
        public void GetById_IdExists_LiftReturned()
        {
            //Arrange
            var expectedId = 1;

            //Act
            var actual = _liftService.GetById(expectedId);

            //Assert
            Assert.AreEqual(expectedId, actual.Id);
        }

        [Test]
        public void GetById_IdDoesNotExist_ExceptionThrown()
        {
            //Arrange
            var expectedId = 99;

            //Act & Assert
            Assert.Throws<NotFoundException>(() => _liftService.GetById(expectedId));
        }

        [Test]
        public void GetLiftLogs_InitialState_LogsEmpty()
        {
            //Arrange
            var liftId = 1;
            var expectedLogLines = 0;

            //Act
            var logs = _liftService.GetLiftLogs(liftId);

            //Act & Assert
            Assert.AreEqual(expectedLogLines, logs.Count());
        }

        [Test]
        [TestCase(3, true)]
        [TestCase(-5, false)]
        public void CallLift_FloorProvided_ResponseCorrect(int floor, bool expectedResult)
        {
            //Arrange
            var liftId = 1;

            //Act
            var actual = _liftService.CallLift(liftId, new CallLiftDto{Floor = floor});

            //Act & Assert
            Assert.AreEqual(expectedResult, actual);
        }

    }
}