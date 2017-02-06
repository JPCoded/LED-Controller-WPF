﻿using System;
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

        #region TextBoxes

        #region TextChanged
        private void txtRed_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = ValueFun.OverUnderValidation(((TextBox)sender).Text);
            
            if (((TextBox) sender).Text == canColor.Red.ToString()) return;
            //Convert textbox to byte, but check to see if it's empty, if so send 0
            var rbyteValue = Convert.ToByte(((TextBox)sender).Text);
            //change red vaule of main color
            canColor.SavedColor = Color.FromRgb(rbyteValue, canColor.Green, canColor.Blue);
        }

        private void txtGreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = ValueFun.OverUnderValidation(((TextBox)sender).Text);

            if (((TextBox) sender).Text == canColor.Green.ToString()) return;
            //Convert textbox to byte, but check to see if it's empty, if so send 0
            var gbyteValue = Convert.ToByte(((TextBox)sender).Text);
            //change green value of main color
            canColor.SavedColor = Color.FromRgb(canColor.Red, gbyteValue, canColor.Blue);
        }

        private void txtBlue_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = ValueFun.OverUnderValidation(((TextBox)sender).Text);

            if (((TextBox) sender).Text == canColor.Blue.ToString()) return;
            //Convert textbox to byte, but check to see if it's empty, if so send 0
            var bbyteValue = Convert.ToByte(((TextBox)sender).Text);
            //change blue vaule of main color
            canColor.SavedColor = Color.FromRgb(canColor.Red, canColor.Green, bbyteValue);
        }

        #endregion

        #region PreviewKeyDown

        private void txtRGB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            ValueFun.KeyPreview(sender,e);
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
            ValueFun.HexKeyValidation(e);
          
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
