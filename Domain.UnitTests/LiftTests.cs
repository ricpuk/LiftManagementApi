using System.Threading.Tasks;
using Domain.Entities;
using NUnit.Framework;

namespace Domain.UnitTests
{
    public class LiftTests
    {
        [Test]
        [TestCase(2, 1)]
        [TestCase(1, 2)]
        public async Task TravelTo_MultipleFloors_FloorChanged(int targetFloor, int currentFloor)
        {
            const double time = 0.1;

            //Arrange
            var lift = new Lift(1, time, time, currentFloor);

            //Act
            await lift.TravelTo(targetFloor);

            //Assert
            Assert.AreEqual(lift.CurrentFloor, targetFloor);
        }
    }
}