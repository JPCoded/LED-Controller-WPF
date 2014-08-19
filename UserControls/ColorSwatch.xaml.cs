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
    ///     Interaction logic for ColorSwatch.xaml
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
        private readonly List<BitmapImage> _images = new List<BitmapImage>();
        //not best idea, but might as well make array of them so i don't have to constanlty make new unsafebitmaps
        private readonly UnsafeBitmap[] _unsafeBitmaps =
        {
            new UnsafeBitmap(Properties.Resources.ColorSwatch),
            new UnsafeBitmap(Properties.Resources.ColorSwatch2), new UnsafeBitmap(Properties.Resources.ColorSwatch3)
        };

        //unsafe bitmap used for functions to find color
        private UnsafeBitmap _myUnsafeBitmap;
        //used to track current bitmap
        private int _tracker;
        //list for the bitmap images

        static ColorSwatch()
        {
            SavedColorProperty = DependencyProperty.Register("SavedColor", typeof (Color), typeof (ColorSwatch),
                new FrameworkPropertyMetadata(Colors.Black, OnColorChanged));

            HoverColorProperty = DependencyProperty.Register("HoverColor", typeof (Color), typeof (ColorSwatch),
                new FrameworkPropertyMetadata(Colors.Black, OnHoverChanged));

            RedProperty = DependencyProperty.Register("Red", typeof (byte), typeof (ColorSwatch),
                new FrameworkPropertyMetadata(OnColorRgbChanged));

            GreenProperty = DependencyProperty.Register("Green", typeof (byte), typeof (ColorSwatch),
                new FrameworkPropertyMetadata(OnColorRgbChanged));

            BlueProperty = DependencyProperty.Register("Blue", typeof (byte), typeof (ColorSwatch),
                new FrameworkPropertyMetadata(OnColorRgbChanged));

            ColorChangedEvent = EventManager.RegisterRoutedEvent("ColorChanged", RoutingStrategy.Bubble,
                typeof (RoutedPropertyChangedEventHandler<Color>), typeof (ColorSwatch));

            HoverChangedEvent = EventManager.RegisterRoutedEvent("HoverChanged", RoutingStrategy.Bubble,
                typeof (RoutedPropertyChangedEventHandler<Color>), typeof (ColorSwatch));
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
            lblTrack.Content = "[" + _tracker + "/2]";
        }

        #region Bitmap functions

        private static bool SimmilarColor(Color pointColor, Color selectedColor)
        {
            int diff = Math.Abs(pointColor.R - selectedColor.R) + Math.Abs(pointColor.G - selectedColor.G) +
                       Math.Abs(pointColor.B - selectedColor.B);
            return diff < 20;
        }

        private Color GetColorFromImage(int i, int j)
        {
            _myUnsafeBitmap.LockBitmap();
            PixelData pixel = _myUnsafeBitmap.GetPixel(i, j);
            var Colorfromimagepoint = Color.FromRgb(pixel.Red, pixel.Green, pixel.Blue);
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
            EpPointer.SetValue(Canvas.LeftProperty, (double) (i - 3));
            EpPointer.SetValue(Canvas.TopProperty, (double) (j - 3));
            EpPointer.InvalidateVisual();
            canColor.InvalidateVisual();
        }

        public void Reposition()
        {
            _myUnsafeBitmap.LockBitmap();

            for (var i = 0; i < canColor.ActualWidth; i++)
            {
                var flag = false;
                for (var j = 0; j < canColor.ActualHeight; j++)
                {
                    try
                    {
                        PixelData pixel = _myUnsafeBitmap.GetPixel(i, j);

                        var Colorfromimagepoint = Color.FromRgb(pixel.Red, pixel.Green, pixel.Blue);
                        if (SimmilarColor(Colorfromimagepoint, SavedColor))
                        {
                            MovePointerDuringReposition(i, j);
                            flag = true;
                            break;
                        }
                    }
                    catch (Exception)
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
                SavedColor = GetColorFromImage((int) Mouse.GetPosition(canColor).X, (int) Mouse.GetPosition(canColor).Y);
                MovePointer();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        private List<BitmapImage> Images
        {
            get { return _images; }
        }

        public Color SavedColor
        {
            get { return (Color) GetValue(SavedColorProperty); }
            set { SetValue(SavedColorProperty, value); }
        }

        public Color HoverColor
        {
            get { return (Color) GetValue(HoverColorProperty); }
            set { SetValue(HoverColorProperty, value); }
        }

        public byte Red
        {
            get { return (byte) GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        public byte Green
        {
            get { return (byte) GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }

        public byte Blue
        {
            get { return (byte) GetValue(BlueProperty); }
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

        public static void OnColorRgbChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var colorSwatch = (ColorSwatch) sender;
            Color color = colorSwatch.SavedColor;
            if (e.Property == RedProperty)
                color.R = (byte) e.NewValue;
            else if (e.Property == GreenProperty)
                color.G = (byte) e.NewValue;
            else if (e.Property == BlueProperty)
                color.B = (byte) e.NewValue;
            colorSwatch.SavedColor = color;
        }

        public static void OnHoverChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var newColor = (Color) e.NewValue;
            var oldColor = (Color) e.OldValue;
            var hoverSwatch = (ColorSwatch) sender;
            hoverSwatch.HoverColor = newColor;
            var args = new RoutedPropertyChangedEventArgs<Color>(oldColor, newColor) {RoutedEvent = HoverChangedEvent};
            hoverSwatch.RaiseEvent(args);
        }

        public static void OnColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var newColor = (Color) e.NewValue;
            var oldColor = (Color) e.OldValue;

            var colorSwatch = (ColorSwatch) sender;
            colorSwatch.Red = newColor.R;
            colorSwatch.Blue = newColor.B;
            colorSwatch.Green = newColor.G;

            var args = new RoutedPropertyChangedEventArgs<Color>(oldColor, newColor) {RoutedEvent = ColorChangedEvent};
            colorSwatch.RaiseEvent(args);
            colorSwatch.Reposition();
        }

        public void DoTrack(char pm)
        {
            switch (pm)
            {
                case '-':
                        _tracker = (_tracker > 0) ? _tracker-1:_tracker;
                        btnPrevious.IsEnabled = (_tracker != 0);
                        btnNext.IsEnabled = true;
                    break;
                case '+':
                        _tracker = (_tracker < 2) ? _tracker+1 : _tracker;
                        btnNext.IsEnabled = (_tracker != 2);
                        btnPrevious.IsEnabled = true;
                    break;
            }

            imgColor.Source = Images[_tracker];
            _myUnsafeBitmap = _unsafeBitmaps[_tracker];
            Reposition();
            lblTrack.Content = "[" + _tracker + "/2]";
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            DoTrack('-');
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            DoTrack('+');
        }

        private void canColor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeColor();
            e.Handled = true;
        }

        private void canColor_MouseMove(object sender, MouseEventArgs e)
        {
            HoverColor = GetColorFromImage((int) Mouse.GetPosition(canColor).X, (int) Mouse.GetPosition(canColor).Y);
        }
    }
}