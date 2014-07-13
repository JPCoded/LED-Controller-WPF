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
        private bool _focus = false;
        public RGBBox()
        {
            InitializeComponent();
        }
        public bool rgbFocus
        {
            get { return _focus; }
            set { _focus = value; }
        }
        public void setColor(int RGB)
        {
            txtRGB.Text = RGB.ToString();
            txtHex.Text = RGB.ToString("X").PadLeft(2, '0');
        }

        /// <summary>
        /// Set the label.
        /// </summary>
        public string LabelText
        {
            get { return lblName.Content.ToString(); }
            set { if (this.lblName.Content.ToString() != value) { this.lblName.Content = value; } }
        }
        /// <summary>
        /// Get or Set the value of txtRGB.
        /// </summary>
        public string Text
        {
            get { return this.txtRGB.Text; }
            set { if(this.txtRGB.Text != value) {this.txtRGB.Text = value;} }
        }
        /// <summary>
        /// Get or Set value of txtRGB as integer.
        /// </summary>
        public int Value
        {
            get
            {
                if (string.IsNullOrEmpty(txtRGB.Text))
                { return 000; }
                else
                {
                    //issue with the space key getting through, so have to have way to catch it until it's figured out
                    try { return Int32.Parse(this.txtRGB.Text); }
                    catch { return 0; }
                }
            }
            set { if (this.txtRGB.Text != value.ToString()) { this.txtRGB.Text = value.ToString(); } }
        }

        public string HexText
        {
            get { return txtHex.Text; }
            set { txtHex.Text = value; }
        }
        private void txtRGB_KeyDown(object sender, KeyEventArgs e)
        {
  
            //not the most elegant way, but was having issues with the +-./*` keys sneaking in the usual way so took different route
            if (e.Key == Key.D || e.Key == Key.N || e.Key == Key.U || e.Key == Key.M || e.Key == Key.P || e.Key == Key.A)
                e.Handled = true;
            else
                e.Handled = !("D1D2D3D4D5D6D7D8D9D0NumPad0NumPad1NumPad2NumPad3NumPad4NumPad5NumPad6NumPad7NumPad8NumPad9".Contains(e.Key.ToString()));
        }

        private void txtRGB_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            //because there's no point in copying this part of code for each text box, i put it hear to save space and make life easier.
            if (string.IsNullOrEmpty(txtRGB.Text))
            { e.Handled = true; }
            else if (Convert.ToInt32(txtRGB.Text) > 255)
            {
                txtRGB.Text = "255";
                txtRGB.CaretIndex = 3; //set cursor to end, else it puts cursor to front of text which isn't what most people would want.
            }

            if (TextChanged != null)
            {
                TextChanged(this, EventArgs.Empty);
            }
        }

        private void txtHex_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtHex_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(this, EventArgs.Empty);
            }
        }

        private void txtRGB_GotFocus(object sender, RoutedEventArgs e)
        {
            rgbFocus = true;
        }

        private void txtRGB_LostFocus(object sender, RoutedEventArgs e)
        {
            rgbFocus = false;
        }


    }
}
