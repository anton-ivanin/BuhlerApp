namespace BuhlerApp.Model
{
    public interface IRecipeSchedulerFactory
    {
        public IRecipeScheduler GetInstance(IMachine machine);
    }
}
