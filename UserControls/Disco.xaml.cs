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
            TxtGMax.Text = "255";
            TxtRMax.Text = "255";
            TxtBMax.Text = "255";

            BtnSet.Click += (sender, e) => Visibility = Visibility.Hidden;
        }

        public byte RedMin => Convert.ToByte(TxtRMin.Text);
        public byte RedMax => Convert.ToByte(TxtRMax.Text);
        public byte GreenMin => Convert.ToByte(TxtGMin.Text);
        public byte GreenMax => Convert.ToByte(TxtGMax.Text);
        public byte BlueMin => Convert.ToByte(TxtBMin.Text);
        public byte BlueMax => Convert.ToByte(TxtBMax.Text);

        private void txtMinMax_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            _valueFun.KeyPreview(sender, e);
        }

        private void txtRed_TextChanged(object sender, TextChangedEventArgs e)
        {
            TxtRMin.Text = _valueFun.OverUnderValidation(TxtRMin.Text, 254);
            TxtRMax.Text = _valueFun.OverUnderValidation(TxtRMax.Text);
            var min = Convert.ToInt32(TxtRMin.Text);
            var max = Convert.ToInt32(TxtRMax.Text);
            if (min > max)
            {
                TxtRMax.Text = (min + 1).ToString();
            }
        }

        private void txtGreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            TxtGMax.Text = _valueFun.OverUnderValidation(TxtGMax.Text);
            TxtGMin.Text = _valueFun.OverUnderValidation(TxtGMin.Text, 254);

            var min = Convert.ToInt32(TxtGMin.Text);
            var max = Convert.ToInt32(TxtGMax.Text);
            if (min > max)
            {
                TxtGMax.Text = (min + 1).ToString();
            }
        }

        private void txtBlue_TextChanged(object sender, TextChangedEventArgs e)
        {
            TxtBMax.Text = _valueFun.OverUnderValidation(TxtBMax.Text);
            TxtBMin.Text = _valueFun.OverUnderValidation(TxtBMin.Text, 254);
            var min = Convert.ToInt32(TxtBMin.Text);
            var max = Convert.ToInt32(TxtBMax.Text);
            if (min > max)
            {
                TxtBMax.Text = (min + 1).ToString();
            }
        }
    }
}