using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_LED_Controller.UserControls
{
    /// <summary>
    /// Interaction logic for RgbTextBox.xaml
    /// </summary>
    public partial class RgbTextBox : UserControl
    {

        public event TextChangedEventHandler TextBoxChanged;
       

        public RgbTextBox()
        {
            InitializeComponent();
        }

        private static string OverUnderValidation(string valueToCheck)
        {
            if (!string.IsNullOrEmpty(valueToCheck))
            {
                var checkVal = Int32.Parse(valueToCheck);
                if (checkVal >= 255)
                { return "255"; }
                if (checkVal < 0)
                { return "0"; }
            }
            else
            { return "0"; }
            return valueToCheck.ToUpper();
        }

        private void RgbBox_PreviewKeyDown(object sender, KeyEventArgs e)
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
            else if (e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9))
            { e.Handled = false; }
            else
            { e.Handled = true; }
        }

        private void RgbBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text);

            ;
            if (TextBoxChanged != null)
            {
                TextBoxChanged(sender, e);
            }
        }
    }
}
