using System.Windows.Controls;
using System.IO.Ports;
using System.Collections.ObjectModel;

namespace WPF_LED_Controller.UserControls
{
    /// <summary>
    /// Interaction logic for PortList.xaml
    /// </summary>
    public partial class PortList : UserControl
    {
        public class Ports
        {
            public Ports(string name)
            { Name = name; }

            private string Name { get; set; }
            //needed because otherwise the output of text into listview isn't correct.
            public override string ToString()
            {
                return Name;
            }
        }
        public string GetPort
        {get {
            
            return (lsPorts.SelectedValue == null)? string.Empty : lsPorts.SelectedValue.ToString();
        } 
        }

        public ObservableCollection<Ports> MyPorts
        { get { return _ports; } }

        private readonly ObservableCollection<Ports> _ports = new ObservableCollection<Ports>();
        public PortList()
        {
            InitializeComponent();
            lsPorts.ItemsSource = MyPorts;
            btnRefresh.Click += (sender, e) => Refresh();
        }

        public void Refresh()
        {
            //clear list just to make sure we don't get duplicates
            MyPorts.Clear();
            for (var i = 0; i < MyPorts.Count; i++)
            { MyPorts.RemoveAt(i); }
            //dump all the port names into MyPorts
            foreach (var port in SerialPort.GetPortNames())
            {
                MyPorts.Add(new Ports(port));
            }
        }
    }
}
