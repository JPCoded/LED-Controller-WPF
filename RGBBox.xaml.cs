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
        //needed to access TextChanged event outside this control
        public event EventHandler<EventArgs> TextChanged;
        public RGBBox()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Get or Set the label
        /// </summary>
        public string Text
        {
            get { return this.lblName.Content.ToString(); }
            set { this.lblName.Content = value; }
        }
        /// <summary>
        /// Get or Set value of txtRGB as integer
        /// </summary>
        public int Value
        {
            get
            {
                if (string.IsNullOrEmpty(txtRGB.Text))
                { return 0; }
                else
                { return Int32.Parse(this.txtRGB.Text); }
            }
            set { this.txtRGB.Text = value.ToString(); }
        }
        private void txtRGB_KeyDown(object sender, KeyEventArgs e)
        {
       
                if (e.Key > Key.D0 || e.Key < Key.D9)
                {
                    e.Handled = false;
                }
        
        }

        private void txtRGB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(this, EventArgs.Empty);
            }
        }


    }
}
