#region

using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#endregion

namespace WPF_LED_Controller
{
    /// <summary>
    ///     Interaction logic for ColorPicker.xaml
    /// </summary>
    internal sealed partial class ColorPicker
    {
        private readonly IValueFun _valueFun = new ValueFun();

        public ColorPicker() => InitializeComponent();

        private void txtRed_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = (TextBox)sender;

            textbox.Text = _valueFun.OverUnderValidation(textbox.Text);

            if (textbox.Text == canColor.Red.ToString()) return;
            var rbyteValue = Convert.ToByte(textbox.Text);

            canColor.SavedColor = Color.FromRgb(rbyteValue, canColor.Green, canColor.Blue);
        }

        private void txtGreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = (TextBox)sender;
            textbox.Text = _valueFun.OverUnderValidation(textbox.Text);

            if (textbox.Text == canColor.Green.ToString()) return;
            //Convert text box to byte, but check to see if it's empty, if so send 0
            var gbyteValue = Convert.ToByte(textbox.Text);
            //change green value of main color
            canColor.SavedColor = Color.FromRgb(canColor.Red, gbyteValue, canColor.Blue);
        }

        private void txtBlue_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = (TextBox)sender;
            textbox.Text = _valueFun.OverUnderValidation(textbox.Text);

            if (textbox.Text == canColor.Blue.ToString()) return;
            //Convert text box to byte, but check to see if it's empty, if so send 0
            var bbyteValue = Convert.ToByte(textbox.Text);
            //change blue value of main color
            canColor.SavedColor = Color.FromRgb(canColor.Red, canColor.Green, bbyteValue);
        }

        private void txtRGB_PreviewKeyDown(object sender, KeyEventArgs e) => _valueFun.KeyPreview(sender, e);

        /// <summary>
        ///     KeyDown for the AllHex textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHAll_KeyDown(object sender, KeyEventArgs e)
        {
            _valueFun.HexKeyValidation(e);

            var strHex = ((TextBox)sender).Text;
            //check to see if it's full hex with either 6 digits (no alpha) or 8 digits (with alpha) plus #, if they aren't, we go no farther in code.
            if ((strHex.Length != 7 && strHex.Length != 9) || strHex[0] != '#') return;

            var convertFromString = ColorConverter.ConvertFromString(strHex);

            if (convertFromString != null)
                canColor.SavedColor = (Color)convertFromString;
        }
    }
}