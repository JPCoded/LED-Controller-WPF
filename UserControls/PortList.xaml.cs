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
    internal sealed partial class PortList
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
            MyPorts.RemoveAll();
            MyPorts.AddArray(SerialPort.GetPortNames());
        }
    }

    internal static class PortsExtension
    {
        public static void RemoveAll(this ObservableCollection<Ports> portCollection)
        {
            foreach (var port in portCollection)
            {
                portCollection.Remove(port);
            }
        }

        public static void AddArray(this ObservableCollection<Ports> portCollection, IEnumerable<string> arrayToAdd)
        {
            foreach (var port in arrayToAdd)
            {
                portCollection.Add(new Ports(port));
            }
        }
    }
}