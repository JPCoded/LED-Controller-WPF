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

        public ColorPicker()
        {
            InitializeComponent();
        }

        #region Validation
      
       

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
        private string OverUnderValidation(string ValueToCheck)
        {
            if (!string.IsNullOrEmpty(ValueToCheck))
            {
                int checkVal = Int32.Parse(ValueToCheck);
                if (checkVal >= 255)
                { return "255"; }
                else if (checkVal < 0)
                { return "0"; }
            }
            else
            { return "0"; }
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
