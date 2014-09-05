using System.ComponentModel;
using System.Windows;
using System.IO.Ports;
using WPF_LED_Controller.UserControls;
using System;

namespace WPF_LED_Controller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly SerialPort  _arduinoSerial = new SerialPort();
        private readonly Disco _discoWindow = new Disco();
        public MainWindow()
        {
            InitializeComponent();
            _arduinoSerial.BaudRate = 115200;
            plPorts.Refresh();
           Closing += (sender, e) => _discoWindow.Close(); ;
            
        }

        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
           if(!string.IsNullOrEmpty(plPorts.GetPort))
           {
               //try and catch any issues that pop up when using the arduino.
               try
               {
                   if (_arduinoSerial.IsOpen)
                   { _arduinoSerial.Close(); }
                  
                   _arduinoSerial.PortName = plPorts.GetPort;
                   
                   _arduinoSerial.Open();

                   try
                   {
                       if (cbDisco.IsChecked == true)
                       {
                          
                            byte[] discoBytes = { _discoWindow.RedMin, _discoWindow.RedMax, _discoWindow.GreenMin, _discoWindow.GreenMax, _discoWindow.BlueMin, _discoWindow.BlueMax, Convert.ToByte(_discoWindow.slDiscoSlider.Value), 0x0A };
                           _arduinoSerial.Write(discoBytes, 0, 7);
                       }
                       else
                       { 
                           byte[] colorBytes = { cpColor.canColor.Red, cpColor.canColor.Green, cpColor.canColor.Blue, 0x0A };
                          _arduinoSerial.Write(colorBytes, 0, 3); 
                       }
                       
                   }
                   catch (System.IO.IOException)
                   {
                       MessageBox.Show("Failed to communicate with arduino. Make sure you have port selected.", "Communication Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                   }
               }
               catch (System.IO.IOException)
               {
                   MessageBox.Show("Error connecting to port " + plPorts.GetPort + ". Make sure the Aruduino is connected or correct port selected.", "IO Error", MessageBoxButton.OK, MessageBoxImage.Error);
               }
               catch(UnauthorizedAccessException)
               {
                   MessageBox.Show("Unauthorized Access to this port.", "Unauthorized Access", MessageBoxButton.OK, MessageBoxImage.Error);
               }
               catch(ArgumentException ae)
               {
                   MessageBox.Show("Message: " + ae.Message, "ArgumentExecption", MessageBoxButton.OK, MessageBoxImage.Error);
               }
               catch(InvalidOperationException io)
               {
                   MessageBox.Show("Message: " + io.Message, "InvalidOperationExecption", MessageBoxButton.OK, MessageBoxImage.Error);
               }
           }
           else
           {
               MessageBox.Show("I can't allow you to do that Dave.\nPlease select a port first.", "Hal9000",MessageBoxButton.OK,MessageBoxImage.Error);
           }
        }

        private void btnDiso_Click(object sender, RoutedEventArgs e)
        {
            _discoWindow.Visibility = _discoWindow.IsVisible ? Visibility.Hidden : Visibility.Visible;
        }
    }

}
