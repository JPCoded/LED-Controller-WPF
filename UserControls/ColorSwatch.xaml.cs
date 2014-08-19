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
    public partial class ColorSwatch 
    {

        public static DependencyProperty RedProperty;
        public static DependencyProperty GreenProperty;
        public static DependencyProperty BlueProperty;
        public static DependencyProperty SavedColorProperty;
        public static DependencyProperty HoverColorProperty;
        public static readonly RoutedEvent ColorChangedEvent;
        public static readonly RoutedEvent HoverChangedEvent;
       //not best idea, but might as well make array of them so i don't have to constanlty make new unsafebitmaps
        private readonly UnsafeBitmap[] _unsafeBitmaps = { new UnsafeBitmap(Properties.Resources.ColorSwatch), new UnsafeBitmap(Properties.Resources.ColorSwatch2), new UnsafeBitmap(Properties.Resources.ColorSwatch3) };
        //unsafe bitmap used for functions to find color
        private UnsafeBitmap _myUnsafeBitmap;
        //used to track current bitmap
        private int _tracker = 0;
        //list for the bitmap images
        private readonly List<BitmapImage> _images = new List<BitmapImage>();
        private List<BitmapImage> Images
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
            var colorSwatch = (ColorSwatch)sender;
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
           var newColor = (Color)e.NewValue;
           var oldColor = (Color)e.OldValue;
           var hoverSwatch = (ColorSwatch)sender;
           hoverSwatch.HoverColor = newColor;
           var args = new RoutedPropertyChangedEventArgs<Color>(oldColor, newColor);
           args.RoutedEvent = HoverChangedEvent;
                hoverSwatch.RaiseEvent(args);
       }
        public static void OnColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
       {
           var newColor = (Color)e.NewValue;
           var oldColor = (Color)e.OldValue;

           var colorSwatch = (ColorSwatch)sender;
           colorSwatch.Red = newColor.R;
           colorSwatch.Blue = newColor.B;
           colorSwatch.Green = newColor.G;

           var args = new RoutedPropertyChangedEventArgs<Color>(oldColor, newColor);
           args.RoutedEvent = ColorChangedEvent;
           colorSwatch.RaiseEvent(args);
           colorSwatch.Reposition();
       }
        
        public void doTrack(char pm)
        {
            if(pm == '-')
            {
                if (_tracker > 0)
                {
                    _tracker--;
                    btnPrevious.IsEnabled = (_tracker == 0) ? false : true;
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
                if (_tracker < 2)
                {
                    _tracker++;
                    btnNext.IsEnabled = (_tracker == 2) ? false : true;
                    btnPrevious.IsEnabled = true;
                }
                else
                {
                    btnNext.IsEnabled = false;
                    btnPrevious.IsEnabled = true;
                }
            }

            imgColor.Source = Images[_tracker];
            _myUnsafeBitmap = _unsafeBitmaps[_tracker];
            Reposition();
            lblTrack.Content = "[" + _tracker.ToString() + "/2]";
        }
   
        public ColorSwatch()
        {
            InitializeComponent();
            Images.Add(new BitmapImage(new Uri(@"/Images/Swatch.png", UriKind.RelativeOrAbsolute)));
            Images.Add(new BitmapImage(new Uri(@"/Images/Swatch2.png", UriKind.RelativeOrAbsolute)));
            Images.Add(new BitmapImage(new Uri(@"/Images/Swatch3.png", UriKind.RelativeOrAbsolute)));
            //set background.
            imgColor.Source = Images[_tracker];
            //set unsafebitmap
            _myUnsafeBitmap = _unsafeBitmaps[0];
            lblTrack.Content = "[" + _tracker.ToString() + "/2]";
        }

        #region Bitmap functions
        private static bool SimmilarColor(Color pointColor, Color selectedColor)
        {
            int diff = Math.Abs(pointColor.R - selectedColor.R) + Math.Abs(pointColor.G - selectedColor.G) + Math.Abs(pointColor.B - selectedColor.B);
            if (diff < 20) return true;
            
                return false;
        }

        private Color GetColorFromImage(int i, int j)
        {
            _myUnsafeBitmap.LockBitmap();
            PixelData pixel = _myUnsafeBitmap.GetPixel(i, j);
            Color Colorfromimagepoint = Color.FromRgb(pixel.Red, pixel.Green, pixel.Blue);
            _myUnsafeBitmap.UnlockBitmap();
            return Colorfromimagepoint;
        }

        private void MovePointer()
        {
            EpPointer.SetValue(Canvas.LeftProperty, (Mouse.GetPosition(canColor).X - 5));
            EpPointer.SetValue(Canvas.TopProperty, (Mouse.GetPosition(canColor).Y - 5));
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
            _myUnsafeBitmap.LockBitmap();

            for (int i = 0; i < canColor.ActualWidth; i++)
            {
                bool flag = false;
                for (int j = 0; j < canColor.ActualHeight; j++)
                {
                    try
                    {
                        PixelData pixel = _myUnsafeBitmap.GetPixel(i, j);

                        Color Colorfromimagepoint = Color.FromRgb(pixel.Red, pixel.Green, pixel.Blue);
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
            _myUnsafeBitmap.UnlockBitmap();
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
