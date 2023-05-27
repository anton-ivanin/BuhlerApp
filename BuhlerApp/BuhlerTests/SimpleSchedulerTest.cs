using BuhlerApp.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BuhlerTests
{
    [TestClass]
    public class SimpleSchedulerTest
    {
        [TestMethod]
        public void CorrectRecipeIsCreated()
        {
            var machine = new Mock<IMachine>();
            var scheduler = new SimpleScheduler(machine.Object);
            DateTime creationTime = DateTime.Now.AddHours(1);
            int duration = 60;

            var recipe = scheduler.Schedule("Test recipe", creationTime, duration);

            Assert.AreEqual(recipe.Machine, machine.Object);
            Assert.AreEqual(recipe.StartTime, creationTime);
            Assert.AreEqual(recipe.DurationMinutes, duration);
        }

        [TestMethod]
        [DataRow(-10)]
        [DataRow(0)]
        public void IncorrectDurationThrowsException(int duration)
        {
            var machine = new Mock<IMachine>();
            var scheduler = new SimpleScheduler(machine.Object);
            DateTime creationTime = DateTime.Now;

            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => scheduler.Schedule("Test recipe", creationTime, duration),
                "Duration should be positive.");
        }

        [TestMethod]
        public void StartTimeExpiredThrowsException()
        {
            var machine = new Mock<IMachine>();
            var scheduler = new SimpleScheduler(machine.Object);
            DateTime creationTime = DateTime.Now;
            int duration = 60;

            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => scheduler.Schedule("Test recipe", creationTime, duration),
                "Cannot schedule recipe to the past time.");
        }

        [TestMethod]
        public void TwoNotOverlappedRecipesAreCreated()
        {
            var machine = new Mock<IMachine>();
            var scheduler = new SimpleScheduler(machine.Object);
            DateTime creationTime1 = DateTime.Now.AddHours(1);
            DateTime creationTime2 = creationTime1.AddHours(1);
            int duration = 60;

            var recipe1 = scheduler.Schedule("Test recipe 1", creationTime1, duration);
            var recipe2 = scheduler.Schedule("Test recipe 2", creationTime2, duration);

            Assert.AreEqual(recipe1.Machine, machine.Object);
            Assert.AreEqual(recipe1.StartTime, creationTime1);
            Assert.AreEqual(recipe1.DurationMinutes, duration);
            Assert.AreEqual(recipe2.Machine, machine.Object);
            Assert.AreEqual(recipe2.StartTime, creationTime2);
            Assert.AreEqual(recipe2.DurationMinutes, duration);
        }

        [TestMethod]
        public void TwoOverlappedRecipesForDifferentMachinesAreCreated()
        {
            var machine1 = new Mock<IMachine>();
            var machine2 = new Mock<IMachine>();
            var scheduler1 = new SimpleScheduler(machine1.Object);
            var scheduler2 = new SimpleScheduler(machine2.Object);
            DateTime creationTime = DateTime.Now.AddHours(1);
            int duration = 60;

            var recipe1 = scheduler1.Schedule("Test recipe 1", creationTime, duration);
            var recipe2 = scheduler2.Schedule("Test recipe 2", creationTime, duration);

            Assert.AreEqual(recipe1.Machine, machine1.Object);
            Assert.AreEqual(recipe1.StartTime, creationTime);
            Assert.AreEqual(recipe1.DurationMinutes, duration);
            Assert.AreEqual(recipe2.Machine, machine2.Object);
            Assert.AreEqual(recipe2.StartTime, creationTime);
            Assert.AreEqual(recipe2.DurationMinutes, duration);
        }

        [TestMethod]
        [DataRow(0, 60)]
        [DataRow(59, 60)]
        [DataRow(-59, 60)]
        [DataRow(65, 120)]
        public void TwoOverlappedRecipesCreationThrowsException(int creationTimeDelta, int duration)
        {
            var machine = new Mock<IMachine>();
            var scheduler = new SimpleScheduler(machine.Object);
            DateTime creationTime1 = DateTime.Now.AddDays(1);
            DateTime creationTime2 = creationTime1.AddMinutes(creationTimeDelta);

            scheduler.Schedule("Test recipe 1", creationTime1, duration);

            Assert.ThrowsException<ArgumentException>(
                () => scheduler.Schedule("Test recipe 2", creationTime2, duration),
                "Selected period is not available.");
        }

        [TestMethod]
        public void GetScheduleReturnsCorrectDataInCorrectOrder()
        {
            var machine = new Mock<IMachine>();
            var scheduler = new SimpleScheduler(machine.Object);
            DateTime creationTime1 = DateTime.Now.AddHours(1);
            DateTime creationTime2 = creationTime1.AddHours(1);
            int duration = 60;

            var recipe2 = scheduler.Schedule("Test recipe 2", creationTime2, duration);
            var recipe1 = scheduler.Schedule("Test recipe 1", creationTime1, duration);

            var schedule = scheduler.GetSchedule().ToList();

            Assert.AreEqual(2, schedule.Count);
            Assert.ReferenceEquals(recipe1, schedule[0]);
            Assert.ReferenceEquals(recipe2, schedule[1]);
        }

        [TestMethod]
        public void CancelRecipeRemovesRecipeFromSchedule()
        {
            var machine = new Mock<IMachine>();
            var scheduler = new SimpleScheduler(machine.Object);
            DateTime creationTime1 = DateTime.Now.AddHours(1);
            DateTime creationTime2 = creationTime1.AddHours(1);
            int duration = 60;

            var recipe1 = scheduler.Schedule("Test recipe 1", creationTime1, duration);
            var recipe2 = scheduler.Schedule("Test recipe 2", creationTime2, duration);

            scheduler.Cancel(recipe1);

            var schedule = scheduler.GetSchedule().ToList();

            Assert.AreEqual(1, schedule.Count);
            Assert.ReferenceEquals(recipe2, schedule[0]);
        }
    }
}