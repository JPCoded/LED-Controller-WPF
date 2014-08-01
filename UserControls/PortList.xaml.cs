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
            public string Name { get; private set; }
            //needed because otherwise the output of text into listview isn't correct.
            public override string ToString()
            {
                return this.Name;
            }
        }
        public string getPort
        {get {
            string portname = string.Empty;
            portname = (lsPorts.SelectedValue == null)? string.Empty : lsPorts.SelectedValue.ToString();
            return portname;
        } 
        }

        public ObservableCollection<Ports> MyPorts
        { get { return _Ports; } }

        private ObservableCollection<Ports> _Ports = new ObservableCollection<Ports>();
        public PortList()
        {
            InitializeComponent();
            lsPorts.ItemsSource = MyPorts;
        }

        public void Refresh()
        {

            //clear list just to make sure we don't get duplicates
            MyPorts.Clear();
            for (int i = 0; i < MyPorts.Count; i++)
            { MyPorts.RemoveAt(i); }
            //dump all the port names into MyPorts
            foreach (string port in SerialPort.GetPortNames())
            {
                MyPorts.Add(new Ports(port));
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}
