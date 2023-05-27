namespace BuhlerApp.Model
{
    public interface IMachine
    {
        public string Name { get; }

        public MachineType Type { get; }
    }
}
