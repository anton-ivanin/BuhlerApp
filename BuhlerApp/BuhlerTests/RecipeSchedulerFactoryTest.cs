using BuhlerApp.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BuhlerTests
{
    [TestClass]
    public class RecipeSchedulerFactoryTest
    {
        [TestMethod]
        public void SingleInstanceIsCreatedForMachine()
        {
            var machine = new Mock<IMachine>();
            var factory = new RecipeSchedulerFactory();

            var scheduler1 = factory.GetInstance(machine.Object);
            var scheduler2 = factory.GetInstance(machine.Object);

            Assert.ReferenceEquals(scheduler1, scheduler2);
        }
    }
}