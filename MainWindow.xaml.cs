using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Ink;
using System.IO.Ports;

namespace WPF_LED_Controller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        SerialPort ArduinoSerial = new SerialPort();

        public MainWindow()
        {
            InitializeComponent();
            ArduinoSerial.BaudRate = 115200;
            plPorts.Refresh();

        }

        private void miOpen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void miSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cpColor_MouseMove(object sender, MouseEventArgs e)
        {

            lblHover.Background = new SolidColorBrush(cpColor.HoverColor);
        }

        private void cpColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //rgbRed.setColor(cpColor.SavedColor.R);
            //rgbGreen.setColor(cpColor.SavedColor.G);
            //rgbBlue.setColor(cpColor.SavedColor.B);
            //lblSaved.Background = new SolidColorBrush(cpColor.SavedColor);
            e.Handled = true;
        }

       // private Color MakeColorFromRGB()
       // {
            //string rsValue = string.IsNullOrEmpty(rgbRed.txtRGB.Text) ? "0" : rgbRed.txtRGB.Text;
            //byte rbyteValue = Convert.ToByte(rsValue);
            //string gsValue = string.IsNullOrEmpty(rgbGreen.txtRGB.Text) ? "0" : rgbGreen.txtRGB.Text;
            //byte gbyteValue = Convert.ToByte(gsValue);
            //string bsValue = string.IsNullOrEmpty(rgbBlue.txtRGB.Text) ? "0" : rgbBlue.txtRGB.Text;
            //byte bbyteValue = Convert.ToByte(bsValue);
            //Color rgbColor = Color.FromRgb(rbyteValue, gbyteValue, bbyteValue);
            //return rgbColor;
       // }

    }


}
