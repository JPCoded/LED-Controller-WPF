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
using System.Collections.ObjectModel;

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

        private void cpColor_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void cpColor_MouseDown(object sender, MouseButtonEventArgs e)
        {

            lblSaved.Background = new SolidColorBrush(cpColor.SavedColor);
        }

        private void cpColor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            lblSaved.Background = new SolidColorBrush(cpColor.SavedColor);
        }

        private void rgbRed_MyTextChanged(object sender, EventArgs e)
        {
            MessageBox.Show("text changed");
        }

        private void rgbRed_TextChanged(object sender, EventArgs e)
        {
            
            if(rgbRed.Value > 255)
            {
                rgbRed.Value = 255;
            }
        }

        private void rgbGreen_TextChanged(object sender, EventArgs e)
        {
            if (rgbGreen.Value > 255)
            {
                rgbGreen.Value = 255;
            }
        }

        private void rgbBlue_TextChanged(object sender, EventArgs e)
        {
            if (rgbBlue.Value > 255)
            {
                rgbBlue.Value = 255;
            }
        }
        private Color MakeColorFromRGB()
        {
            byte rbyteValue = Convert.ToByte(rgbRed.Text);
            byte gbyteValue = Convert.ToByte(rgbGreen.Text);
            byte bbyteValue = Convert.ToByte(rgbBlue.Text);
            Color rgbColor = Color.FromRgb(rbyteValue, gbyteValue, bbyteValue);
            return rgbColor;
            
        }   

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
           plPorts.Refresh();
        }
    }

 
}
