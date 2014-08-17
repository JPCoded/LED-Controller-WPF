using System;
using System.Collections.Generic;
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
    public partial class ColorSwatch : UserControl
    {

        public static DependencyProperty RedProperty;
        public static DependencyProperty GreenProperty;
        public static DependencyProperty BlueProperty;
        public static DependencyProperty SavedColorProperty;
        public static DependencyProperty HoverColorProperty;
        public static readonly RoutedEvent ColorChangedEvent;
        public static readonly RoutedEvent HoverChangedEvent;
       //not best idea, but might as well make array of them so i don't have to constanlty make new unsafebitmaps
        private UnsafeBitmap[] unsafeBitmaps = { new UnsafeBitmap(WPF_LED_Controller.Properties.Resources.ColorSwatch), new UnsafeBitmap(WPF_LED_Controller.Properties.Resources.ColorSwatch2), new UnsafeBitmap(WPF_LED_Controller.Properties.Resources.ColorSwatch3) };
        //unsafe bitmap used for functions to find color
        private UnsafeBitmap myUnsafeBitmap;
        //private colors
        private Color _hoverColor = Colors.Transparent;
        //used to track current bitmap
        private int Tracker = 0;
        //list for the bitmap images
        private List<BitmapImage> _images = new List<BitmapImage>();
        private List<BitmapImage> images
        { get { return _images; } }
        static ColorSwatch()
        { 
            SavedColorProperty = DependencyProperty.Register("SavedColor", typeof(Color), typeof(ColorSwatch), new FrameworkPropertyMetadata(Colors.Black, new PropertyChangedCallback(OnColorChanged)));
           
            HoverColorProperty = DependencyProperty.Register("HoverColor", typeof(Color), typeof(ColorSwatch), new FrameworkPropertyMetadata(Colors.Black, new PropertyChangedCallback(OnHoverChanged)));
          
            RedProperty = DependencyProperty.Register("Red", typeof(byte), typeof(ColorSwatch), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnColorRGBChanged)));

           GreenProperty = DependencyProperty.Register("Green", typeof(byte), typeof(ColorSwatch), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnColorRGBChanged)));

            BlueProperty = DependencyProperty.Register("Blue", typeof(byte), typeof(ColorSwatch), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnColorRGBChanged)));

            ColorChangedEvent = EventManager.RegisterRoutedEvent("ColorChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Color>), typeof(ColorSwatch));
           
            HoverChangedEvent = EventManager.RegisterRoutedEvent("HoverChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Color>), typeof(ColorSwatch));

        }
        public Color SavedColor
        {
            get { return (Color)GetValue(SavedColorProperty); }
            set { SetValue(SavedColorProperty, value); }
        }

        public Color HoverColor
        {
            get { return (Color)GetValue(HoverColorProperty); }
            set { SetValue(HoverColorProperty, value); }
        }
        
        public byte Red
        {
            get { return (byte)GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        public byte Green     
        {
            get { return (byte)GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }

        public byte Blue
        {
            get { return (byte)GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }

        public event RoutedPropertyChangedEventHandler<Color> ColorChanged
        {
            add { AddHandler(ColorChangedEvent, value); }
            remove { RemoveHandler(ColorChangedEvent, value); }
        }

        public event RoutedPropertyChangedEventHandler<Color> HoverChanged
        {
            add { AddHandler(HoverChangedEvent, value); }
            remove { RemoveHandler(HoverChangedEvent, value); }
        }
       public static void OnColorRGBChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ColorSwatch colorSwatch = (ColorSwatch)sender;
            Color color = colorSwatch.SavedColor;
            if (e.Property == RedProperty)
                color.R = (byte)e.NewValue;
            else if (e.Property == GreenProperty)
                color.G = (byte)e.NewValue;
            else if (e.Property == BlueProperty)
                color.B = (byte)e.NewValue;
            colorSwatch.SavedColor = color;    
        }

        public static void OnHoverChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
       {
           Color newColor = (Color)e.NewValue;
           Color oldColor = (Color)e.OldValue;
           ColorSwatch hoverSwatch = (ColorSwatch)sender;
           hoverSwatch.HoverColor = newColor;
           RoutedPropertyChangedEventArgs<Color> args = new RoutedPropertyChangedEventArgs<Color>(oldColor, newColor);
           args.RoutedEvent = ColorSwatch.HoverChangedEvent;
                hoverSwatch.RaiseEvent(args);
       }
        public static void OnColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
       {
           Color newColor = (Color)e.NewValue;
           Color oldColor = (Color)e.OldValue;

           ColorSwatch colorSwatch = (ColorSwatch)sender;
           colorSwatch.Red = newColor.R;
           colorSwatch.Blue = newColor.B;
           colorSwatch.Green = newColor.G;

           RoutedPropertyChangedEventArgs<Color> args = new RoutedPropertyChangedEventArgs<Color>(oldColor, newColor);
           args.RoutedEvent = ColorSwatch.ColorChangedEvent;
           colorSwatch.RaiseEvent(args);
           colorSwatch.Reposition();
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

        public void Reposition()
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
                        if (SimmilarColor(Colorfromimagepoint, SavedColor))
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
                SavedColor = GetColorFromImage((int)Mouse.GetPosition(canColor).X, (int)Mouse.GetPosition(canColor).Y);
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
                if (SavedColor != newColor)
                {
                    SavedColor = newColor;
                    Reposition();
                }
            }
            catch
            {
                return;
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
