using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF_LED_Controller.UserControls
{
    /// <summary>
    /// Interaction logic for ColorSwatch.xaml
    /// </summary>
    public partial class ColorSwatch : UserControl, INotifyPropertyChanged
    {

        public static DependencyProperty RedProperty;
        public static DependencyProperty GreenProperty;
        public static DependencyProperty BlueProperty;
        public static DependencyProperty ColorProperty;
        public static readonly RoutedEvent ColorChangedEvent;

        static ColorSwatch()
        {
            ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(ColorSwatch), new FrameworkPropertyMetadata(Colors.Black, new PropertyChangedCallback(OnColorChanged)));

            RedProperty = DependencyProperty.Register("Red1", typeof(byte), typeof(ColorSwatch), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnColorRGBChanged)));

           GreenProperty = DependencyProperty.Register("Green1", typeof(byte), typeof(ColorSwatch), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnColorRGBChanged)));

            BlueProperty = DependencyProperty.Register("Blue1", typeof(byte), typeof(ColorSwatch), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnColorRGBChanged)));

            ColorChangedEvent = EventManager.RegisterRoutedEvent("ColorChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Color>), typeof(ColorSwatch));

        }
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        
        public byte Red1
        {
            get { return (byte)GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        public byte Green1       
        {
            get { return (byte)GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }

        public byte Blue1
        {
            get { return (byte)GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }

        public event RoutedPropertyChangedEventHandler<Color> ColorChanged
        {
            add { AddHandler(ColorChangedEvent, value); }
            remove { RemoveHandler(ColorChangedEvent, value); }
        }
       public static void OnColorRGBChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ColorSwatch colorSwatch = (ColorSwatch)sender;
            Color color = colorSwatch.Color;
            if (e.Property == RedProperty)
                color.R = (byte)e.NewValue;
            else if (e.Property == GreenProperty)
                color.G = (byte)e.NewValue;
            else if (e.Property == BlueProperty)
                color.B = (byte)e.NewValue;

            colorSwatch.Color = color;
        }

        public static void OnColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
       {
           Color newColor = (Color)e.NewValue;
           Color oldColor = (Color)e.OldValue;

           ColorSwatch colorSwatch = (ColorSwatch)sender;
           colorSwatch.Red1 = newColor.R;
           colorSwatch.Blue1 = newColor.B;
           colorSwatch.Green1 = newColor.G;

           RoutedPropertyChangedEventArgs<Color> args = new RoutedPropertyChangedEventArgs<Color>(oldColor, newColor);
           args.RoutedEvent = ColorSwatch.ColorChangedEvent;
           colorSwatch.RaiseEvent(args);
       }

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
        private List<BitmapImage> images
        { get { return _images; } }
        
        public Color CustomColor
        {
            get
            { return _customColor; }
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
            { return _hoverColor; }
            private set
            {
                if (_hoverColor != value)
                {
                    _hoverColor = value;
                    NotifyPropertyChanged("Hover");
                }
            }
        }

        //can't get converters to work right, so just making Red, Green, and Blue Functions to make it easier to convert colors to string and hex 
        public string Red(bool Hex = false)
        {
            if(Hex)
            { return CustomColor.R.ToString("X").PadLeft(2, '0'); }
            else
            { return CustomColor.R.ToString(); }
        }
        public string Green(bool Hex = false)
        {
            if (Hex)
            { return CustomColor.G.ToString("X").PadLeft(2, '0'); }
            else
            { return CustomColor.G.ToString(); }
        }
        public string Blue(bool Hex = false)
        {
            if (Hex)
            { return CustomColor.B.ToString("X").PadLeft(2, '0'); }
            else
            { return CustomColor.B.ToString(); }
        }

        public void doTrack(char pm)
        {
            if(pm == '-')
            {
                if (Tracker > 0)
                {
                    Tracker--;
                    btnPrevious.IsEnabled = (Tracker == 0) ? false : true;
                    btnNext.IsEnabled = true;
                }
                else 
                {
                    btnNext.IsEnabled = true;
                    btnPrevious.IsEnabled = false;
                }
            
            }
            else if(pm == '+')
            {
                if (Tracker < 2)
                {
                    Tracker++;
                    btnNext.IsEnabled = (Tracker == 2) ? false : true;

                    btnPrevious.IsEnabled = true;
                }
                else
                {
                    btnNext.IsEnabled = false;
                    btnPrevious.IsEnabled = true;
                }
            }

            imgColor.Source = images[Tracker];
            myUnsafeBitmap = unsafeBitmaps[Tracker];
            Reposition();
            lblTrack.Content = "[" + Tracker.ToString() + "/2]";
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
                Color = CustomColor;
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
                    Color = newColor;
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
            doTrack('-');
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            doTrack('+');
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
