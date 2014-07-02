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

namespace WPF_LED_Controller
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        private bool IsMouseDownOverEllipse = false;
        private bool _shift = false;

        private Color _customColor = Colors.Transparent;

        public Color CustomColor
        {
            get { return _customColor; }
            set
            {
                if (_customColor != value)
                {
                    _customColor = value;
                }
            }
        }

        public Color HoverColor { get; set; }
        public Color SavedColor { get; set; }
        public ColorPicker()
        {
            InitializeComponent();
            image.Source = loadBitmap(WPF_LED_Controller.Properties.Resources.ColorSwatch);
        }


        #region Custom Functions

        private Color GetColorFromImage(int i, int j)
        {
            CroppedBitmap cb = new CroppedBitmap(image.Source as BitmapSource,
       new Int32Rect(i,
           j, 1, 1));
            byte[] color = new byte[4];
            cb.CopyPixels(color, 4, 0);
            Color Colorfromimagepoint =
              Color.FromArgb((byte)255, color[2], color[1], color[0]);
            return Colorfromimagepoint;
        }

        //find similar colors since there's isn't the full range of colors
        private bool SimmilarColor(Color pointColor, Color selectedColor)
        {
            int diff = Math.Abs(pointColor.R - selectedColor.R) + Math.Abs(pointColor.G - selectedColor.G) + Math.Abs(pointColor.B - selectedColor.B);
            if (diff < 20) return true;
            else
                return false;
        }

        public static BitmapSource loadBitmap(System.Drawing.Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(source.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }

        private void MovePointer()
        {
            EpPointer.SetValue(Canvas.LeftProperty, (double)(Mouse.GetPosition(CanColor).X - 5));
            EpPointer.SetValue(Canvas.TopProperty, (double)(Mouse.GetPosition(CanColor).Y - 5));
            CanColor.InvalidateVisual();
        }

        private void MovePointerDuringReposition(int i, int j)
        {
            EpPointer.SetValue(Canvas.LeftProperty, (double)(i - 3));
            EpPointer.SetValue(Canvas.TopProperty, (double)(j - 3));
            EpPointer.InvalidateVisual();
            CanColor.InvalidateVisual();
        }

        private void Reposition()
        {
            for (int i = 0; i < CanColor.ActualWidth; i++)
            {
                bool flag = false;
                for (int j = 0; j < CanColor.ActualHeight; j++)
                {
                    try
                    {
                        Color Colorfromimagepoint = GetColorFromImage(i, j);
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
        }

        private void ChangeColor()
        {
            try
            {
                CustomColor = GetColorFromImage((int)Mouse.GetPosition(CanColor).X, (int)Mouse.GetPosition(CanColor).Y);
                SavedColor = CustomColor;
                MovePointer();
            }
            catch
            {
            }
        } 
        #endregion

        #region epPointer Functions
        private void EpPointer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsMouseDownOverEllipse = false;
        }

        private void EpPointer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsMouseDownOverEllipse = true;
        }

        private void EpPointer_MouseMove(object sender, MouseEventArgs e)
        {
            if(IsMouseDownOverEllipse)
            {
                ChangeColor();
            }
            e.Handled = true;
        }
        #endregion

        #region canColor Functions
        private void CanColor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeColor();
            e.Handled = true;
        }

        private void CanColor_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void CanColor_MouseMove(object sender, MouseEventArgs e)
        {
            HoverColor = GetColorFromImage((int)Mouse.GetPosition(CanColor).X, (int)Mouse.GetPosition(CanColor).Y);
        }
        #endregion


    }
}
