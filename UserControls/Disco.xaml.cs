﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_LED_Controller.UserControls
{
    /// <summary>
    /// Interaction logic for Disco.xaml
    /// </summary>
    public partial class Disco : Window
    {
      
        public Disco()
        {
           
            InitializeComponent();
           txtGMax.Text = "255";
            txtRMax.Text = "255";
            txtBMax.Text = "255";

            btnSet.Click += (sender, e) => Visibility = Visibility.Hidden;

        }


        public byte RedMin => Convert.ToByte(txtRMin.Text);

        public byte RedMax => Convert.ToByte(txtRMax.Text);

        public byte GreenMin => Convert.ToByte(txtGMin.Text);

        public byte GreenMax => Convert.ToByte(txtGMax.Text);

        public byte BlueMin => Convert.ToByte(txtBMin.Text);

        public byte BlueMax => Convert.ToByte(txtBMax.Text);


        private void txtMinMax_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            ValueFun.KeyPreview(sender,e);
        }

        private void txtGMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtGMin.Text = ValueFun.OverUnderValidation(txtGMin.Text,254);
            txtGMax.Text = ValueFun.OverUnderValidation(txtGMax.Text);
            var min = Convert.ToInt32(txtGMin.Text);
           
            var max = Convert.ToInt32(txtGMax.Text);
            if (min > max)
            {
                txtGMax.Text = (min + 1).ToString();
            }
        }

        private void txtGMax_TextChanged(object sender, TextChangedEventArgs e)
        {
           txtGMax.Text = ValueFun.OverUnderValidation(txtGMax.Text);
           txtGMin.Text = ValueFun.OverUnderValidation(txtGMin.Text,254);
            
            var min = Convert.ToInt32(txtGMin.Text);
            var max = Convert.ToInt32(txtGMax.Text);
            if (min > max)
            {
               txtGMax.Text = (min + 1).ToString();
            }
        }

        private void txtBMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtBMin.Text = ValueFun.OverUnderValidation(txtBMin.Text,254);
            txtBMax.Text = ValueFun.OverUnderValidation(txtBMax.Text);
            var min = Convert.ToInt32(txtBMin.Text);
            var max = Convert.ToInt32(txtBMax.Text);
            if (min > max)
            {
                txtBMax.Text = (min + 1).ToString();
            }
        }

        private void txtBMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtBMax.Text = ValueFun.OverUnderValidation(txtBMax.Text);
            txtBMin.Text = ValueFun.OverUnderValidation(txtBMin.Text,254);
            var min = Convert.ToInt32(txtBMin.Text);
            var max = Convert.ToInt32(txtBMax.Text);
            if (min > max)
            {
                txtBMax.Text = (min + 1).ToString();
            }
        }

        private void txtRMin_TextChanged(object sender, TextChangedEventArgs e)
        {

            txtRMin.Text = ValueFun.OverUnderValidation(txtRMin.Text,254);
            txtRMax.Text = ValueFun.OverUnderValidation(txtRMax.Text);
            var min = Convert.ToInt32(txtRMin.Text);
            var max = Convert.ToInt32(txtRMax.Text);
            if (min > max)
            {
                txtRMax.Text = (min + 1).ToString();
            }
        }

        private void txtRMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtRMin.Text = ValueFun.OverUnderValidation(txtRMin.Text,254);
            txtRMax.Text = ValueFun.OverUnderValidation(txtRMax.Text);
            var min = Convert.ToInt32(txtRMin.Text);
            var max = Convert.ToInt32(txtRMax.Text);
            if (min > max)
            {
                txtRMax.Text = (min + 1).ToString();
            }
        }
    }
}
