using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            cpColor.txtRed.TextChanged += txtRed_TextChanged;
            cpColor.txtGreen.TextChanged += txtGreen_TextChanged;
            cpColor.txtBlue.TextChanged += txtBlue_TextChanged;
            pwlSaved.SetTitle = "Saved";
            pwlHover.SetTitle = "Hover";
            ArduinoSerial.BaudRate = 115200;
            plPorts.Refresh();


        }

        void txtBlue_TextChanged(object sender, TextChangedEventArgs e)
        {
           pwlSaved.setBackground(cpColor.canColor.CustomColor);
        }

        void txtGreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            
             pwlSaved.setBackground(cpColor.canColor.CustomColor);
        }

        void txtRed_TextChanged(object sender, TextChangedEventArgs e)
        {
            pwlSaved.setBackground(cpColor.canColor.CustomColor);
        }

        private void miOpen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void miSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cpColor_MouseMove(object sender, MouseEventArgs e)
        {
            pwlHover.setBackground(cpColor.canColor.HoverColor);
        }

        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
           if(!string.IsNullOrEmpty(plPorts.getPort))
           {
               //try and catch any issues that pop up when using the arduino.
               try
               {
                   if (ArduinoSerial.IsOpen)
                   { ArduinoSerial.Close(); }
                  
                   ArduinoSerial.PortName = plPorts.getPort;
                   
                   ArduinoSerial.Open();
                   byte[] colorBytes = { cpColor.canColor.CustomColor.R, cpColor.canColor.CustomColor.G, cpColor.canColor.CustomColor.B, 0x0A };
                   try
                   {
                       ArduinoSerial.Write(colorBytes, 0, 3);
                   }
                   catch (System.IO.IOException)
                   {
                       MessageBox.Show("Failed to communicate with arduino. Make sure you have port selected.", "Communication Failure");
                   }
               }
               catch (System.IO.IOException)
               {
                   MessageBox.Show("Error connecting to port " + plPorts.getPort + ". Make sure the Aruduino is connected or correct port selected.", "IO Error");
               }
               catch(System.UnauthorizedAccessException)
               {
                   MessageBox.Show("Unauthorized Access to this port.", "Unauthorized Access");
               }
               catch(System.ArgumentException ae)
               {
                   MessageBox.Show("Message: " + ae.Message, "ArgumentExecption");
               }
               catch(System.InvalidOperationException io)
               {
                   MessageBox.Show("Message: " + io.Message,"InvalidOperationExecption");
               }
           }
           else
           {
               MessageBox.Show("I can't allow you to do that Dave.\nPlease select a port first.", "Hal9000");
           }
        }
    }

}
