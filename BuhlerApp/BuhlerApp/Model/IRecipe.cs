namespace BuhlerApp.Model
{
    public interface IRecipe
    {
        public string Name { get; }

        public IMachine Machine { get; }

        public DateTime StartTime { get; }

        public int DurationMinutes { get; }
    }
}
