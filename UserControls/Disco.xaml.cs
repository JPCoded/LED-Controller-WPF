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
            txtGMax.Text = Properties.Resources.ColorMax;
            txtRMax.Text = Properties.Resources.ColorMax;
            txtBMax.Text = Properties.Resources.ColorMax;

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

        private void txtRed_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtGreen_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtBlue_TextChanged(object sender, TextChangedEventArgs e)
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
    }
}