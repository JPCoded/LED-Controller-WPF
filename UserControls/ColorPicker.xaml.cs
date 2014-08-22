using System;
using System.Globalization;
using System.Windows.Controls;
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
        public ColorPicker()
        {
            InitializeComponent();
        }

        #region Validation
        private static void HexKeyValidation(KeyEventArgs e)
        {
            var input = e.Key.ToString();
            if (e.Key == Key.D3 && (e.Key == Key.LeftShift || e.Key == Key.RightShift))
            { input = "#"; }
            if (!(input == "#" || (input[0] >= 'A' && input[0] <= 'F') || (input[0] >= 'a' && input[0] <= 'F') || (input[0] >= '0' && input[0] <= '9')))
            {
                e.Handled = true;
            }
        }
        private static string OverUnderValidation(string valueToCheck)
        {
            if (!string.IsNullOrEmpty(valueToCheck))
            {
                int checkVal = Int32.Parse(valueToCheck);
                if (checkVal >= 255)
                { return "255"; }
                if (checkVal < 0)
                { return "0"; }
            }
            else
            { return "0"; }
            return valueToCheck.ToUpper();
        }
        #endregion

        #region TextBoxes

        #region TextChanged
        private void txtRed_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text);

            if (((TextBox) sender).Text == canColor.Red.ToString(CultureInfo.InvariantCulture)) return;
            //Convert textbox to byte, but check to see if it's empty, if so send 0
            var rbyteValue = Convert.ToByte(((TextBox)sender).Text);
            //change red vaule of main color
            canColor.SavedColor = Color.FromRgb(rbyteValue, canColor.Green, canColor.Blue);
        }

        private void txtGreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text);

            if (((TextBox) sender).Text == canColor.Green.ToString(CultureInfo.InvariantCulture)) return;
            //Convert textbox to byte, but check to see if it's empty, if so send 0
            var gbyteValue = Convert.ToByte(((TextBox)sender).Text);
            //change green vaule of main color
            canColor.SavedColor = Color.FromRgb(canColor.Red, gbyteValue, canColor.Blue);
        }

        private void txtBlue_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text);

            if (((TextBox) sender).Text == canColor.Blue.ToString(CultureInfo.InvariantCulture)) return;
            //Convert textbox to byte, but check to see if it's empty, if so send 0
            var bbyteValue = Convert.ToByte(((TextBox)sender).Text);
            //change blue vaule of main color
            canColor.SavedColor = Color.FromRgb(canColor.Red, canColor.Green, bbyteValue);
        }

        #endregion

        #region PreviewKeyDown
        /// <summary>
        /// Checks whether the Up or Down key has been pressed, or if spacebar pressed (causes error later in code if not caught)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRGB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (string.IsNullOrEmpty(((TextBox)sender).Text))
                { ((TextBox)sender).Text = "0"; }
                var oldValue = Convert.ToInt32(((TextBox)sender).Text);
                var newValue = (oldValue + 1 > 255) ? 255 : oldValue + 1;
                ((TextBox)sender).Text = newValue.ToString(CultureInfo.InvariantCulture);
            }
            else if (e.Key == Key.Down)
            {
                if (string.IsNullOrEmpty(((TextBox)sender).Text))
                { ((TextBox)sender).Text = "255"; }
                var oldValue = Convert.ToInt32(((TextBox)sender).Text);
                var newValue = (oldValue - 1 < 0) ? 0 : oldValue - 1;
                ((TextBox)sender).Text = newValue.ToString(CultureInfo.InvariantCulture);
            }
            else if(e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9))
            { e.Handled = false; }
            else
            { e.Handled = true; }
        }

        #endregion

        #region KeyDown

        /// <summary>
        /// KeyDown for the AllHex textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHAll_KeyDown(object sender, KeyEventArgs e)
        {
            HexKeyValidation(e);
          
                var strHex = ((TextBox) sender).Text;
                //check to see if it's full hex with either 6 digits (no alpha) or 8 digits (with alpha) plus #, if they arent, we go no farther in code.
            if ((strHex.Length != 7 && strHex.Length != 9) || strHex[0] != '#') return;
           
            var convertFromString = ColorConverter.ConvertFromString(strHex);
            if (convertFromString != null)
                canColor.SavedColor = (Color) convertFromString;
        }
        #endregion

        #endregion     
    }
}
