namespace BuhlerApp.Model
{
    internal class GenericRecipe : IRecipe
    {
        public string Name { get; }

        public IMachine Machine { get; }

        public DateTime StartTime { get; }

        public int DurationMinutes { get; }

        public DateTime EndTime => StartTime.AddMinutes(DurationMinutes);

        public GenericRecipe(string name, IMachine machine, DateTime startTime, int durationMinutes)
        {
            Name = name;
            Machine = machine;
            StartTime = startTime;
            DurationMinutes = durationMinutes;
        }

        public override string ToString()
        {
            return $"{Name}. Starts at {StartTime}. Finishes at {EndTime}.";
        }
    }
}
