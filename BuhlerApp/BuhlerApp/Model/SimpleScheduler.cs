namespace BuhlerApp.Model
{
    public class SimpleScheduler : IRecipeScheduler
    {

        private readonly IMachine machine;

        private readonly SortedList<DateTime, IRecipe> recipes;

        private readonly object lockObject = new object();

        public SimpleScheduler(IMachine machine)
        {
            this.machine = machine;
            recipes = new SortedList<DateTime, IRecipe>();
        }

        public IRecipe Schedule(string name, DateTime startTime, int duration)
        {
            if (duration <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(duration), "Duration should be positive.");
            }

            if (startTime < DateTime.Now)
            {
                throw new ArgumentOutOfRangeException(nameof(startTime), "Cannot schedule recipe to the past time.");
            }

            IRecipe recipe;

            lock (lockObject)
            {
                if (!IsTimePeriodAvailable(startTime, duration))
                {
                    throw new ArgumentException("Selected period is not available");
                }
                
                recipe = CreateReceipe(name, startTime, duration);
                recipes.Add(startTime, recipe);
            }

            return recipe;
        }

        public void Cancel(IRecipe recipe)
        {
            lock (lockObject)
            {
                recipes.Remove(recipe.StartTime);
            }
        }

        public IEnumerable<IRecipe> GetSchedule()
        {
            return recipes.Values;
        }

        private bool IsTimePeriodAvailable(DateTime startTime, int duration)
        {
            if (recipes.ContainsKey(startTime))
            {
                return false;
            }

            IRecipe previous = recipes.LastOrDefault(r => r.Key < startTime).Value;

            if (previous != null && startTime < previous.StartTime.AddMinutes(previous.DurationMinutes))
            {
                return false;
            }

            IRecipe next = recipes.FirstOrDefault(r => r.Key > startTime).Value;

            if (next != null && startTime.AddMinutes(duration) > next.StartTime)
            {
                return false;
            }

            return true;
        }

        private IRecipe CreateReceipe(string name, DateTime startTime, int duration)
        {
            return new GenericRecipe(name, machine, startTime, duration);
        }
    }
}
