namespace BuhlerApp.Model
{
    internal class GenericMachine : IMachine
    {
        public string Name { get; }

        public MachineType Type { get; }

        public GenericMachine(string name, MachineType type)
        {
            Name = name; 
            Type = type;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
