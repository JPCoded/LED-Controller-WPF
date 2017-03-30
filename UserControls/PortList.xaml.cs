#region

using System.Collections.Generic;
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
            LsPorts.ItemsSource = MyPorts;
            BtnRefresh.Click += (sender, e) => Refresh();
        }

        public string GetPort => LsPorts.SelectedValue?.ToString() ?? string.Empty;
        public ObservableCollection<Ports> MyPorts { get; } = new ObservableCollection<Ports>();

        public void Refresh()
        {
 
            MyPorts.RemoveAll();
            MyPorts.AddArray(SerialPort.GetPortNames());
        }
    }

    internal static class PortsExtension
    {
        public static ObservableCollection<Ports> RemoveAll(this ObservableCollection<Ports> portCollection)
        {
            for (var i = 0; i < portCollection.Count; i++)
            {
                portCollection.RemoveAt(i);
            }
            return portCollection;
        }

        public static ObservableCollection<Ports> AddArray(this ObservableCollection<Ports> portCollection,
            IEnumerable<string> arrayToAdd)
        {
            foreach (var port in arrayToAdd)
            {
                portCollection.Add(new Ports(port));
            }
            return portCollection;
        }
    }
}