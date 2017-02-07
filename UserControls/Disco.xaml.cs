#region

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#endregion

namespace WPF_LED_Controller
{
    /// <summary>
    ///     Interaction logic for Disco.xaml
    /// </summary>
    public partial class Disco
    {
        private readonly IValueFun _valueFun = new ValueFun();
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
            _valueFun.KeyPreview(sender, e);
        }

        private void txtGMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtGMin.Text = _valueFun.OverUnderValidation(txtGMin.Text, 254);
            txtGMax.Text = _valueFun.OverUnderValidation(txtGMax.Text);
            var min = Convert.ToInt32(txtGMin.Text);

            var max = Convert.ToInt32(txtGMax.Text);
            if (min > max)
            {
                txtGMax.Text = (min + 1).ToString();
            }
        }

        private void txtGMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtGMax.Text = _valueFun.OverUnderValidation(txtGMax.Text);
            txtGMin.Text = _valueFun.OverUnderValidation(txtGMin.Text, 254);

            var min = Convert.ToInt32(txtGMin.Text);
            var max = Convert.ToInt32(txtGMax.Text);
            if (min > max)
            {
                txtGMax.Text = (min + 1).ToString();
            }
        }

        private void txtBMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtBMin.Text = _valueFun.OverUnderValidation(txtBMin.Text, 254);
            txtBMax.Text = _valueFun.OverUnderValidation(txtBMax.Text);
            var min = Convert.ToInt32(txtBMin.Text);
            var max = Convert.ToInt32(txtBMax.Text);
            if (min > max)
            {
                txtBMax.Text = (min + 1).ToString();
            }
        }

        private void txtBMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtBMax.Text = _valueFun.OverUnderValidation(txtBMax.Text);
            txtBMin.Text = _valueFun.OverUnderValidation(txtBMin.Text, 254);
            var min = Convert.ToInt32(txtBMin.Text);
            var max = Convert.ToInt32(txtBMax.Text);
            if (min > max)
            {
                txtBMax.Text = (min + 1).ToString();
            }
        }

        private void txtRMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtRMin.Text = _valueFun.OverUnderValidation(txtRMin.Text, 254);
            txtRMax.Text = _valueFun.OverUnderValidation(txtRMax.Text);
            var min = Convert.ToInt32(txtRMin.Text);
            var max = Convert.ToInt32(txtRMax.Text);
            if (min > max)
            {
                txtRMax.Text = (min + 1).ToString();
            }
        }

        private void txtRMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtRMin.Text = _valueFun.OverUnderValidation(txtRMin.Text, 254);
            txtRMax.Text = _valueFun.OverUnderValidation(txtRMax.Text);
            var min = Convert.ToInt32(txtRMin.Text);
            var max = Convert.ToInt32(txtRMax.Text);
            if (min > max)
            {
                txtRMax.Text = (min + 1).ToString();
            }
        }
    }
}