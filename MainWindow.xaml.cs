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

namespace WPF_LED_Controller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            lblSaved.Background = new SolidColorBrush(cpColor.SavedColor);
        }

        private void cpColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lblSaved.Background = new SolidColorBrush(cpColor.SavedColor);
        }

        private void cpColor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            lblSaved.Background = new SolidColorBrush(cpColor.SavedColor);
        }
    }
}
