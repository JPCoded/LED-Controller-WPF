using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;

namespace WPF_LED_Controller
{
    /// <summary>
    /// Interaction logic for PortList.xaml
    /// </summary>
    public partial class PortList : UserControl
    {
     
        public class Ports
        { 
            public string Name { get; set; }
            //needed because otherwise the output of text into listview isn't correct.
            public override string ToString()
            {
                return this.Name;
            }
        }

        public List<Ports> MyPorts
        { get { return _Ports; } }

        List<Ports> _Ports = new List<Ports>();
        public PortList()
        {
            InitializeComponent();
            lsPorts.ItemsSource = MyPorts;
        }

        public void Refresh()
        {
            //clear list just to make sure we don't get duplicates
            MyPorts.Clear();
            foreach (string port in SerialPort.GetPortNames())
            {
                MyPorts.Add(new Ports { Name = port });
            }
        }
    }
}
