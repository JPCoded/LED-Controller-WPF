#region

using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace WPF_LED_Controller
{
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