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
using System.Drawing.Imaging;
using WPF_LED_Controller.UserControls;

namespace WPF_LED_Controller.UserControls
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// 
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        
        private Color _customColor = Colors.Transparent;
        public event EventHandler<EventArgs> TextChanged;

        /// <summary>
        /// Color that the mouse is current hovered over.
        /// </summary>
        public Color HoverColor { get; private set; }
        public ColorPicker()
        {
            InitializeComponent();
            canColor.PropertyChanged += canColor_PropertyChanged;
           
           
        }

        void canColor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Custom")
            {
                txtRed.Text = canColor.Red();
                txtRHex.Text = canColor.Red(true);
                txtGreen.Text = canColor.Green();
                txtGHex.Text = canColor.Green(true);
                txtBlue.Text = canColor.Blue();
                txtBHex.Text = canColor.Blue(true);
                txtHAll.Text = String.Format("#{0}{1}{2}", txtRHex.Text, txtGHex.Text, txtBHex.Text);
            }
        }

        #region Validation
        /// <summary>
        /// Check to see if the user inputed keys 0-9, including the numpad keys, but nothing else
        /// </summary>
        /// <param name="e">KeyEventArgs</param>
        private void NumericValidation(KeyEventArgs e)
        {
            //not the most elegant way, but was having issues with the +-./*` keys sneaking in the usual way so took different route
            if (e.Key == Key.D || e.Key == Key.N || e.Key == Key.U || e.Key == Key.M || e.Key == Key.P || e.Key == Key.A)
            {
                e.Handled = true;
            }
            else
            {
                //wanted to have the ability to use numpad too.
                e.Handled = !("D1D2D3D4D5D6D7D8D9D0NumPad0NumPad1NumPad2NumPad3NumPad4NumPad5NumPad6NumPad7NumPad8NumPad9".Contains(e.Key.ToString()));
            }
        }

        private void HexValidation(KeyEventArgs e)
        {
            //for some reason, this way works here, but won't work for the NumericValidation. 
            string input = e.Key.ToString();
            if (e.Key == Key.D3 && (e.Key == Key.LeftShift || e.Key == Key.RightShift))
            { input = "#"; }
            if (!(input == "#" || (input[0] >= 'A' && input[0] <= 'F') || (input[0] >= 'a' && input[0] <= 'F') || (input[0] >= '0' && input[0] <= '9')))
            {
                e.Handled = true;
            }

        }
        private string OverUnderValidation(string ValueToCheck, string valType = "Int")
        {
            if (!string.IsNullOrEmpty(ValueToCheck))
            {
                if (valType == "Int")
                {
                    int checkVal = Int32.Parse(ValueToCheck);
                    if (checkVal > 255)
                    { return "255"; }
                    else if (checkVal < 0)
                    { return "0"; }
                }
                else if(valType == "Hex")
                {
                    int checkVal = Int32.Parse(ValueToCheck, System.Globalization.NumberStyles.HexNumber);
                    if (checkVal > 255)
                    { return (255).ToString("X").PadLeft(2, '0').ToUpper(); }
                    else if (checkVal < 0)
                    { return (0).ToString("X").PadLeft(2, '0').ToUpper(); }
                }
            }
            return ValueToCheck.ToUpper();
        }
        #endregion

        #region TextBoxes

        #region TextChanged
        private void txtRed_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text);

            if (((TextBox)sender).Text != canColor.Red())
            {
                //Convert textbox to byte, but check to see if it's empty, if so send 0
                byte rbyteValue = Convert.ToByte(string.IsNullOrEmpty(((TextBox)sender).Text) ? "0" : ((TextBox)sender).Text);
                //change red vaule of main color
                canColor.ChangeColor(Color.FromRgb(rbyteValue, canColor.CustomColor.G, canColor.CustomColor.B));
            }
            if (TextChanged != null)
            {
                TextChanged(this, EventArgs.Empty);
            }
        }

        private void txtGreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text);

            if (((TextBox)sender).Text != canColor.Green())
            {
                //Convert textbox to byte, but check to see if it's empty, if so send 0
                byte gbyteValue = Convert.ToByte(string.IsNullOrEmpty(((TextBox)sender).Text) ? "0" : ((TextBox)sender).Text);
                //change green vaule of main color
                canColor.ChangeColor(Color.FromRgb(canColor.CustomColor.R, gbyteValue, canColor.CustomColor.B));
            }
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text);

            if (TextChanged != null)
            {
             
                TextChanged(this, EventArgs.Empty);
            }
        }

        private void txtBlue_TextChanged(object sender, TextChangedEventArgs e)
        {

            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text);

            if (((TextBox)sender).Text != canColor.Blue())
            {
                //Convert textbox to byte, but check to see if it's empty, if so send 0
                byte bbyteValue = Convert.ToByte(string.IsNullOrEmpty(((TextBox)sender).Text) ? "0" : ((TextBox)sender).Text);
                //change blue vaule of main color
                canColor.ChangeColor(Color.FromRgb(canColor.CustomColor.R, canColor.CustomColor.G, bbyteValue));
            }
            if (TextChanged != null)
            {
                TextChanged(this, EventArgs.Empty);
            }
        }
        private void txtGHex_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text,"Hex");

            if (TextChanged != null)
            {

                TextChanged(this, EventArgs.Empty);
            }
        }
        private void txtRHex_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text, "Hex");

            if (TextChanged != null)
            {

                TextChanged(this, EventArgs.Empty);
            }
        }

        private void txtBHex_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text, "Hex");

            if (TextChanged != null)
            {

                TextChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region PreviewKeyDown
        private void txtRGB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (string.IsNullOrEmpty(((TextBox)sender).Text))
                { ((TextBox)sender).Text = "0"; }
                int newValue = Convert.ToInt32(((TextBox)sender).Text) + 1;
                ((TextBox)sender).Text = newValue.ToString();
            }
            else if (e.Key == Key.Down)
            {
                if (string.IsNullOrEmpty(((TextBox)sender).Text))
                { ((TextBox)sender).Text = "0"; }
                int newValue = Convert.ToInt32(((TextBox)sender).Text) - 1;
                ((TextBox)sender).Text = newValue.ToString();
            }
        }

        private void txtHex_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (string.IsNullOrEmpty(((TextBox)sender).Text))
                { ((TextBox)sender).Text = "0"; }
                int newValue = Int32.Parse(((TextBox)sender).Text, System.Globalization.NumberStyles.HexNumber) + 1;
                ((TextBox)sender).Text = newValue.ToString("X").PadLeft(2, '0');
            }
            else if (e.Key == Key.Down)
            {
                if (string.IsNullOrEmpty(((TextBox)sender).Text))
                { ((TextBox)sender).Text = "0"; }
                int newValue = Int32.Parse(((TextBox)sender).Text,System.Globalization.NumberStyles.HexNumber) - 1;
                ((TextBox)sender).Text = newValue.ToString("X").PadLeft(2, '0');
            }
        }
        #endregion

        #region KeyDown

        private void txtBlue_KeyDown(object sender, KeyEventArgs e)
        {
            NumericValidation(e);
            string bsValue = string.IsNullOrEmpty(((TextBox)sender).Text) ? "0" : ((TextBox)sender).Text;
            byte bbyteValue = Convert.ToByte(bsValue);
            canColor.ChangeColor(Color.FromRgb(canColor.CustomColor.R, canColor.CustomColor.G, bbyteValue));
        }

        private void txtGreen_KeyDown(object sender, KeyEventArgs e)
        {
            NumericValidation(e);
            string gsValue = string.IsNullOrEmpty(((TextBox)sender).Text) ? "0" : ((TextBox)sender).Text;
            byte gbyteValue = Convert.ToByte(gsValue);
            canColor.ChangeColor(Color.FromRgb(canColor.CustomColor.R, gbyteValue, canColor.CustomColor.B));
        }

        private void txtRed_KeyDown(object sender, KeyEventArgs e)
        {
            NumericValidation(e);
            string rsValue = string.IsNullOrEmpty(((TextBox)sender).Text) ? "0" : ((TextBox)sender).Text;
            byte rbyteValue = Convert.ToByte(rsValue);
            canColor.ChangeColor(Color.FromRgb(rbyteValue, canColor.CustomColor.G, canColor.CustomColor.B));
        }

        private void txtHAll_KeyDown(object sender, KeyEventArgs e)
        {
            HexValidation(e);
            try
            {
                string strHex = ((TextBox)sender).Text;
                if (strHex.Length == 7 && strHex[0] == '#')
                {
                    canColor.CustomColor = (Color)ColorConverter.ConvertFromString(txtHAll.Text); 
                }
            }
            catch 
            { }
        }

        private void txtRHex_KeyDown(object sender, KeyEventArgs e)
        {
            HexValidation(e);
            string rsValue = string.IsNullOrEmpty(((TextBox)sender).Text) ? "0" : ((TextBox)sender).Text;
        }

        private void txtGHex_KeyDown(object sender, KeyEventArgs e)
        {
            HexValidation(e);
            string gsValue = string.IsNullOrEmpty(((TextBox)sender).Text) ? "0" : ((TextBox)sender).Text;
        }

        private void txtBHex_KeyDown(object sender, KeyEventArgs e)
        {
            HexValidation(e);
            string bsValue = string.IsNullOrEmpty(((TextBox)sender).Text) ? "0" : ((TextBox)sender).Text;

        }
        #endregion

        #endregion

      
    }
}
