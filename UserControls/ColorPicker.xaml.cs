using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF_LED_Controller.UserControls
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// 
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public event EventHandler<EventArgs> TextChanged;

        /// <summary>
        /// Color that the mouse is current hovered over.
        /// </summary>
        public Color HoverColor { get; private set; }
        public ColorPicker()
        {
            InitializeComponent();
            canColor.ColorChanged += canColor_ColorChanged;
        }

        void canColor_ColorChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<Color> e)
        { // txtRed.Text = canColor.Red();
               // txtRHex.Text = canColor.Red(true);
                //txtGreen.Text = canColor.Green();
                //txtGHex.Text = canColor.Green(true);
               // txtBlue.Text = canColor.Blue();
                //txtBHex.Text = canColor.Blue(true);
              //  txtHAll.Text = String.Format("#{0}{1}{2}", txtRHex.Text, txtGHex.Text, txtBHex.Text);
              //  canColor.Reposition();
           
        }

        #region Validation
        /// <summary>
        /// Check to see if the user inputed keys 0-9, including the numpad keys, but nothing else
        /// </summary>
        /// <param name="e">KeyEventArgs</param>
        private void RGBKeyValidation(KeyEventArgs e)
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

        private void HexKeyValidation(KeyEventArgs e)
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
                    if (checkVal >= 255)
                    { return "255"; }
                    else if (checkVal < 0)
                    { return "0"; }
                }
                else if(valType == "Hex")
                {
                    int checkVal = Int32.Parse(ValueToCheck, System.Globalization.NumberStyles.HexNumber);
                    if (checkVal >= 255)
                    { return (255).ToString("X").PadLeft(2, '0').ToUpper(); }
                    else if (checkVal < 0)
                    { return (0).ToString("X").PadLeft(2, '0').ToUpper(); }
                }
            }
            else
            {
                if(valType == "Int")
                { return "0";}
                else
                { return (0).ToString("X").PadLeft(2, '0').ToUpper(); }
            }
            return ValueToCheck.ToUpper();
        }
        #endregion

        #region TextBoxes

        #region TextChanged
        private void txtRed_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text);

            if (((TextBox)sender).Text != canColor.Red.ToString())
            {
                //Convert textbox to byte, but check to see if it's empty, if so send 0
                byte rbyteValue = Convert.ToByte(((TextBox)sender).Text);
                //change red vaule of main color
                canColor.ChangeColor(Color.FromRgb(rbyteValue, canColor.Green, canColor.Blue));
            }
            if (TextChanged != null)
            {
                TextChanged(this, EventArgs.Empty);
            }
        }

        private void txtGreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text);

            if (((TextBox)sender).Text != canColor.Green.ToString())
            {
                //Convert textbox to byte, but check to see if it's empty, if so send 0
                byte gbyteValue = Convert.ToByte(((TextBox)sender).Text);
                //change green vaule of main color
                canColor.ChangeColor(Color.FromRgb(canColor.Red, gbyteValue, canColor.Blue));
            }
           
            if (TextChanged != null)
            {
                TextChanged(this, EventArgs.Empty);
            }
        }

        private void txtBlue_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text);

            if (((TextBox)sender).Text != canColor.Blue.ToString())
            {
                //Convert textbox to byte, but check to see if it's empty, if so send 0
                byte bbyteValue = Convert.ToByte(((TextBox)sender).Text);
                //change blue vaule of main color
                canColor.ChangeColor(Color.FromRgb(canColor.Red, canColor.Green, bbyteValue));
            }
            if (TextChanged != null)
            { TextChanged(this, EventArgs.Empty); }
        }

        #endregion

        #region PreviewKeyDown
        private void txtRGB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (string.IsNullOrEmpty(((TextBox)sender).Text))
                { ((TextBox)sender).Text = "0"; }
                int oldValue = Convert.ToInt32(((TextBox)sender).Text);
                int newValue = (oldValue + 1 > 255) ? 255 : oldValue + 1;
                ((TextBox)sender).Text = newValue.ToString();
            }
            else if (e.Key == Key.Down)
            {
                if (string.IsNullOrEmpty(((TextBox)sender).Text))
                { ((TextBox)sender).Text = "255"; }
                int oldValue = Convert.ToInt32(((TextBox)sender).Text);
                int newValue = (oldValue - 1 < 0) ? 0 : oldValue - 1;
                ((TextBox)sender).Text = newValue.ToString();
            }
        }

        #endregion

        #region KeyDown
        /// <summary>
        /// KeyDown for all the RGB textboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRGB_KeyDown(object sender, KeyEventArgs e)
        {
            RGBKeyValidation(e);
        }

        /// <summary>
        /// KeyDown for the AllHex textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHAll_KeyDown(object sender, KeyEventArgs e)
        {
            HexKeyValidation(e);
            try
            {
                string strHex = ((TextBox)sender).Text;
                if (strHex.Length == 7 && strHex[0] == '#')
                {
                    canColor.SavedColor = (Color)ColorConverter.ConvertFromString(txtHAll.Text); 
                }
            }
            catch 
            { }
        }
        #endregion

        #endregion     
    }
}
