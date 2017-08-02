namespace WPF_LED_Controller
{
    internal sealed class Ports : IPorts
    {
        public Ports(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString() => Name;
    }
}