#region

using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Input;

#endregion

namespace WPF_LED_Controller.UserControls
{
    internal static class ValueFun
    {
        public static string OverUnderValidation(string valueToCheck, int max = 255)
        {
            if (!string.IsNullOrEmpty(valueToCheck))
            {
                var checkVal = int.Parse(valueToCheck);
                if (checkVal >= max)
                {
                    return max.ToString();
                }
                if (checkVal < 0)
                {
                    return "0";
                }
            }
            else
            {
                return "0";
            }
            return valueToCheck.ToUpper();
        }

        public static void KeyPreview(object sender, KeyEventArgs e, int max = 255)
        {
            switch (e.Key)
            {
                case Key.Up:
                {
                    if (string.IsNullOrEmpty(((TextBox) sender).Text))
                    {
                        ((TextBox) sender).Text = "0";
                    }
                    var oldValue = Convert.ToInt32(((TextBox) sender).Text);
                    var newValue = (oldValue + 1 > max) ? max : oldValue + 1;
                    ((TextBox) sender).Text = newValue.ToString();
                }
                    break;
                case Key.Down:
                {
                    if (string.IsNullOrEmpty(((TextBox) sender).Text))
                    {
                        ((TextBox) sender).Text = max.ToString();
                    }
                    var oldValue = Convert.ToInt32(((TextBox) sender).Text);
                    var newValue = (oldValue - 1 < 0) ? 0 : oldValue - 1;
                    ((TextBox) sender).Text = newValue.ToString();
                }
                    break;
                default:
                    if (e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right ||
                        (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9))
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                    break;
            }
        }

        public static void HexKeyValidation(KeyEventArgs e)
        {
            var input = e.Key.ToString();
            if (e.Key == Key.D3 && (e.Key == Key.LeftShift || e.Key == Key.RightShift))
            {
                input = "#";
            }
            if (
                !(input == "#" || (input[0] >= 'A' && input[0] <= 'F') || (input[0] >= 'a' && input[0] <= 'F') ||
                  (input[0] >= '0' && input[0] <= '9')))
            {
                e.Handled = true;
            }
        }
    }
}