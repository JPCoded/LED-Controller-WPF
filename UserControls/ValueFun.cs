using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Input;


namespace WPF_LED_Controller.UserControls
{
    static class ValueFun
    {
        public static string OverUnderValidation(string valueToCheck, int max = 255)
        {
            if (!string.IsNullOrEmpty(valueToCheck))
            {
                var checkVal = Int32.Parse(valueToCheck);
                if (checkVal >= max)
                { return max.ToString(CultureInfo.InvariantCulture); }
                if (checkVal < 0)
                { return "0"; }
            }
            else
            { return "0"; }
            return valueToCheck.ToUpper();
        }

        public static void KeyPreview(object sender, KeyEventArgs e, int max = 255)
        {
            if (e.Key == Key.Up)
            {
                if (string.IsNullOrEmpty(((TextBox)sender).Text))
                { ((TextBox)sender).Text = "0"; }
                var oldValue = Convert.ToInt32(((TextBox)sender).Text);
                var newValue = (oldValue + 1 > max) ? max : oldValue + 1;
                ((TextBox)sender).Text = newValue.ToString(CultureInfo.InvariantCulture);
            }
            else if (e.Key == Key.Down)
            {
                if (string.IsNullOrEmpty(((TextBox)sender).Text))
                { ((TextBox)sender).Text = max.ToString(CultureInfo.InvariantCulture); }
                var oldValue = Convert.ToInt32(((TextBox)sender).Text);
                var newValue = (oldValue - 1 < 0) ? 0 : oldValue - 1;
                ((TextBox)sender).Text = newValue.ToString(CultureInfo.InvariantCulture);
            }
            else if (e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9))
            { e.Handled = false; }
            else
            { e.Handled = true; }
        }
    }
}
