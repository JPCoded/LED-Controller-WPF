using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WPF_LED_Controller.UserControls
{
    /// <summary>
    /// Interaction logic for ColorSwatch.xaml
    /// </summary>
    public partial class ColorSwatch : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //not best idea, but might as well make array of them so i don't have to constanlty make new unsafebitmaps
        private UnsafeBitmap[] unsafeBitmaps = { new UnsafeBitmap(WPF_LED_Controller.Properties.Resources.ColorSwatch), new UnsafeBitmap(WPF_LED_Controller.Properties.Resources.ColorSwatch2), new UnsafeBitmap(WPF_LED_Controller.Properties.Resources.ColorSwatch3) };
        //unsafe bitmap used for functions to find color
        private UnsafeBitmap myUnsafeBitmap;
        //private colors
        private Color _customColor = Colors.Transparent;
        private Color _hoverColor = Colors.Transparent;
        //used to track current bitmap
        private int Tracker = 0;
        //list for the bitmap images
        private List<BitmapImage> _images = new List<BitmapImage>();
        public List<BitmapImage> images
        { get { return _images; } }
       
        
        public Color CustomColor
        {
            get
            {
                return _customColor;
            }
             set
            {
                if (_customColor != value)
                {
                    _customColor = value;
                    NotifyPropertyChanged("Custom");
                }
            }
        }
        
        public Color HoverColor
        {
            get
            {
                return _hoverColor;
            }
            private set
            {
                if (_hoverColor != value)
                {
                    _hoverColor = value;
                    NotifyPropertyChanged("Hover");
                }
            }
        }

         private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public ColorSwatch()
        {
            InitializeComponent();
            images.Add(new BitmapImage(new Uri(@"/Images/Swatch.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri(@"/Images/Swatch2.png", UriKind.RelativeOrAbsolute)));
            images.Add(new BitmapImage(new Uri(@"/Images/Swatch3.png", UriKind.RelativeOrAbsolute)));
            //set background.
            imgColor.Source = images[Tracker];
            //set unsafebitmap
            myUnsafeBitmap = unsafeBitmaps[0];
            lblTrack.Content = "[" + Tracker.ToString() + "/2]";
        }
        #region Bitmap functions
        private bool SimmilarColor(Color pointColor, Color selectedColor)
        {
            int diff = Math.Abs(pointColor.R - selectedColor.R) + Math.Abs(pointColor.G - selectedColor.G) + Math.Abs(pointColor.B - selectedColor.B);
            if (diff < 20) return true;
            else
                return false;
        }

        private Color GetColorFromImage(int i, int j)
        {
            myUnsafeBitmap.LockBitmap();
            PixelData pixel = myUnsafeBitmap.GetPixel(i, j);
            Color Colorfromimagepoint = Color.FromRgb(pixel.red, pixel.green, pixel.blue);
            myUnsafeBitmap.UnlockBitmap();
            return Colorfromimagepoint;
        }

        private void MovePointer()
        {
            EpPointer.SetValue(Canvas.LeftProperty, (double)(Mouse.GetPosition(canColor).X - 5));
            EpPointer.SetValue(Canvas.TopProperty, (double)(Mouse.GetPosition(canColor).Y - 5));
            canColor.InvalidateVisual();
        }

        private void MovePointerDuringReposition(int i, int j)
        {
            EpPointer.SetValue(Canvas.LeftProperty, (double)(i - 3));
            EpPointer.SetValue(Canvas.TopProperty, (double)(j - 3));
            EpPointer.InvalidateVisual();
            canColor.InvalidateVisual();
        }

        private void Reposition()
        {
            myUnsafeBitmap.LockBitmap();

            for (int i = 0; i < canColor.ActualWidth; i++)
            {
                bool flag = false;
                for (int j = 0; j < canColor.ActualHeight; j++)
                {
                    try
                    {
                        PixelData pixel = myUnsafeBitmap.GetPixel(i, j);

                        Color Colorfromimagepoint = Color.FromRgb(pixel.red, pixel.green, pixel.blue);
                        if (SimmilarColor(Colorfromimagepoint, _customColor))
                        {

                            MovePointerDuringReposition(i, j);
                            flag = true;
                            break;
                        }
                    }
                    catch
                    {

                    }
                }
                if (flag) break;
            }
            myUnsafeBitmap.UnlockBitmap();
        }


        private void ChangeColor()
        {
            try
            {
                CustomColor = GetColorFromImage((int)Mouse.GetPosition(canColor).X, (int)Mouse.GetPosition(canColor).Y);
                MovePointer();
            }
            catch
            {
                return;
            }
        }

        public void ChangeColor(Color newColor)
        {
            try
            {
                if (CustomColor != newColor)
                {
                    CustomColor = newColor;
                    Reposition();
                }
            }
            catch
            {
                //probably not needed but better safe than sorry
            }
        }
        #endregion

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if(Tracker > 0)
            {
                Tracker--;
                imgColor.Source = images[Tracker];
                myUnsafeBitmap = unsafeBitmaps[Tracker];
                Reposition();
                lblTrack.Content = "[" + Tracker.ToString() + "/2]";
            }
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if(Tracker < 2)
            {
                Tracker++;
                imgColor.Source = images[Tracker];
                myUnsafeBitmap = unsafeBitmaps[Tracker];
                Reposition();
                lblTrack.Content = "[" + Tracker.ToString() + "/2]";
            }
        }

        private void canColor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeColor();
            e.Handled = true;
        }

        private void canColor_MouseMove(object sender, MouseEventArgs e)
        {
            HoverColor = GetColorFromImage((int)Mouse.GetPosition(canColor).X, (int)Mouse.GetPosition(canColor).Y);
        }
    }
}
