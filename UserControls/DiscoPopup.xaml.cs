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

namespace WPF_LED_Controller.UserControls
{
    /// <summary>
    /// Interaction logic for DiscoPopup.xaml
    /// </summary>
    public partial class DiscoPopup : UserControl
    {
        public DiscoPopup()
        {
            InitializeComponent();
        }

        private void btnAdvanced_Click(object sender, RoutedEventArgs e)
        {
           Popup.IsOpen = Popup.IsOpen == false;
        }
    }
}
