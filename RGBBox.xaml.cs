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

namespace WPF_LED_Controller
{
    /// <summary>
    /// Interaction logic for RGBBox.xaml
    /// </summary>
    public partial class RGBBox : UserControl
    {
        public event TextChangedEventHandler TextChanged;
        public RGBBox()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return this.lblName.Content.ToString(); }
            set { this.lblName.Content = value; }
        }
        private void txtRGB_KeyDown(object sender, KeyEventArgs e)
        {
            string input = e.Key.ToString().Substring(1);
            try
            {
                if (string.IsNullOrWhiteSpace(input))
                { e.Handled = false; }
                if (input[0] < '0' || input[0] > '9')
                { e.Handled = false; }
            }
            catch { e.Handled = true; }
        }

        private void txtRGB_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChangedEventHandler h = TextChanged;
            if (h != null)
            {
                h(this, e);
            }
        }

       
    }
}
