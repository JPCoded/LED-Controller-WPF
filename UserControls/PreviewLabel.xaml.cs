using System.Windows.Controls;
using System.Windows.Media;

namespace WPF_LED_Controller.UserControls
{
    /// <summary>
    /// Interaction logic for PreviewLabel.xaml
    /// </summary>
    public partial class PreviewLabel : UserControl
    {
        public PreviewLabel()
        {
            InitializeComponent();
        }
        public string SetTitle
        {
            set { lblTitle.Content = value; }
        }

        public void setBackground(Color background)
        {
            recColor.Fill = new SolidColorBrush(background);
                
        }
    }
   
}
