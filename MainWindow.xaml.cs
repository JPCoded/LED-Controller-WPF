﻿#region

using System;
using System.IO;
using System.IO.Ports;
using System.Windows;
using static WPF_LED_Controller.Properties.Resources;

#endregion

namespace WPF_LED_Controller
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    internal sealed partial class MainWindow
    {
        private readonly SerialPort _arduinoSerial = new SerialPort();
        private readonly Disco _discoWindow = new Disco();
        private const int Offset = 0;
        private const byte EndingByte = 0x0A;
        private const int BaudRate = 115200;

        public MainWindow()
        {
            InitializeComponent();
            _arduinoSerial.BaudRate = BaudRate;
            plPorts.Refresh();
            Closing += (sender, e) => _discoWindow.Close();
        }

        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(plPorts.GetPort))
            {
                //try and catch any issues that pop up when using the arduino.
                try
                {
                    if (_arduinoSerial.IsOpen)
                    {
                        _arduinoSerial.Close();
                    }

                    _arduinoSerial.PortName = plPorts.GetPort;

                    _arduinoSerial.Open();

                    try
                    {
                        if (cbDisco.IsChecked == true)
                        {
                            byte[] discoBytes =
                            {
                                _discoWindow.RedMin, _discoWindow.RedMax, _discoWindow.GreenMin,
                                _discoWindow.GreenMax, _discoWindow.BlueMin, _discoWindow.BlueMax,
                                Convert.ToByte(_discoWindow.slDiscoSlider.Value), EndingByte
                            };
                            _arduinoSerial.Write(discoBytes, Offset, 7);
                        }
                        else
                        {
                            byte[] colorBytes =
                            {
                                cpColor.canColor.Red, cpColor.canColor.Green, cpColor.canColor.Blue,
                                EndingByte
                            };
                            _arduinoSerial.Write(colorBytes, Offset, 3);
                        }
                    }
                    catch (IOException)
                    {
                        MessageBox.Show(strCommunicationFailureMsg, strCommunicationFailure, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show(
                        $"Error connecting to port {plPorts.GetPort}. Make sure the Arduino is connected or correct port selected.", "IO Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Unauthorized Access to this port.", "Unauthorized Access", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                catch (ArgumentException ae)
                {
                    MessageBox.Show(strMessage + ae.Message, "ArgumentException", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                catch (InvalidOperationException io)
                {
                    MessageBox.Show(strMessage + io.Message, "InvalidOperationException", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(strHal9000Msg, strHal9000,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDiso_Click(object sender, RoutedEventArgs e) => _discoWindow.Visibility = _discoWindow.IsVisible ? Visibility.Hidden : Visibility.Visible;
    }
}