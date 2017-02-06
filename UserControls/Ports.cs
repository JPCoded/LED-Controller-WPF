using WPF_LED_Controller.Interfaces;

namespace WPF_LED_Controller.UserControls
{
    public class Ports : IPorts
    {
        public Ports(string name)
        {
            Name = name;
        }

        private string Name { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}