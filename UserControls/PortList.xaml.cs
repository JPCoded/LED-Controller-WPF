#region

using System.Collections.ObjectModel;
using System.IO.Ports;

#endregion

namespace WPF_LED_Controller
{
    /// <summary>
    ///     Interaction logic for PortList.xaml
    /// </summary>
    public partial class PortList
    {
        public PortList()
        {
            InitializeComponent();
            lsPorts.ItemsSource = MyPorts;
            btnRefresh.Click += (sender, e) => Refresh();
        }

        public string GetPort => lsPorts.SelectedValue?.ToString() ?? string.Empty;

        public ObservableCollection<Ports> MyPorts { get; } = new ObservableCollection<Ports>();

        public void Refresh()
        {
            //clear list just to make sure we don't get duplicates
            MyPorts.Clear();

            for (var i = 0; i < MyPorts.Count; i++)
            {
                MyPorts.RemoveAt(i);
            }
            //dump all the port names into MyPorts
            foreach (var port in SerialPort.GetPortNames())
            {
                MyPorts.Add(new Ports(port));
            }
        }


    }
}