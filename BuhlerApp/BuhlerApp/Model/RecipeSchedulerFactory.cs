namespace BuhlerApp.Model
{
    public class RecipeSchedulerFactory : IRecipeSchedulerFactory
    {
        private static Dictionary<IMachine, SimpleScheduler> currentInstances = new Dictionary<IMachine, SimpleScheduler>();
        
        public IRecipeScheduler GetInstance(IMachine machine)
        {
            lock (currentInstances)
            {
                SimpleScheduler scheduler;

                if (!currentInstances.TryGetValue(machine, out scheduler))
                {
                    scheduler = new SimpleScheduler(machine);
                }

                return scheduler;
            }
        }
    }
}
