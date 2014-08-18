using System.Windows;

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

        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
           if(!string.IsNullOrEmpty(plPorts.GetPort))
           {
               //try and catch any issues that pop up when using the arduino.
               try
               {
                   if (ArduinoSerial.IsOpen)
                   { ArduinoSerial.Close(); }
                  
                   ArduinoSerial.PortName = plPorts.GetPort;
                   
                   ArduinoSerial.Open();
                   byte[] colorBytes = { cpColor.canColor.Red, cpColor.canColor.Green, cpColor.canColor.Blue, 0x0A };
                   try
                   {
                       ArduinoSerial.Write(colorBytes, 0, 3);
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
               catch(System.UnauthorizedAccessException)
               {
                   MessageBox.Show("Unauthorized Access to this port.", "Unauthorized Access", MessageBoxButton.OK, MessageBoxImage.Error);
               }
               catch(System.ArgumentException ae)
               {
                   MessageBox.Show("Message: " + ae.Message, "ArgumentExecption", MessageBoxButton.OK, MessageBoxImage.Error);
               }
               catch(System.InvalidOperationException io)
               {
                   MessageBox.Show("Message: " + io.Message, "InvalidOperationExecption", MessageBoxButton.OK, MessageBoxImage.Error);
               }
           }
           else
           {
               MessageBox.Show("I can't allow you to do that Dave.\nPlease select a port first.", "Hal9000",MessageBoxButton.OK,MessageBoxImage.Error);
           }
        }
    }

}
