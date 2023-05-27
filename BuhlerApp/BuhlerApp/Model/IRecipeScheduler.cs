namespace BuhlerApp.Model
{
    public interface IRecipeScheduler
    {
        public IRecipe Schedule(string name, DateTime startTime, int duration);

        public void Cancel(IRecipe recipe);

        public IEnumerable<IRecipe> GetSchedule();
    }
}
