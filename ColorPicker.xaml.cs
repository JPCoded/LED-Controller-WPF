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
    /// Added my RGBBox control into this one. While it makes this code much longer because of added control, it makes it so I don't have to keep making calls from 2 different controls that are to be used by each other so often.
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        private Color _customColor = Colors.Transparent;
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
                }
            }
        }
        private bool canFocus { get; set; }
       /// <summary>
       /// Color that the mouse is current hovered over.
       /// </summary>
        public Color HoverColor { get; private set; }
        public Color SavedColor { get; private set; }
        public ColorPicker()
        {
            InitializeComponent();
            image.Source = loadBitmap(WPF_LED_Controller.Properties.Resources.ColorSwatch);
        }

        #region Custom Functions

        private Color GetColorFromImage(int i, int j)
        {
            CroppedBitmap cb = new CroppedBitmap(image.Source as BitmapSource, new Int32Rect(i, j, 1, 1));
            byte[] color = new byte[4];
            cb.CopyPixels(color, 4, 0);
            Color Colorfromimagepoint = Color.FromArgb((byte)255, color[2], color[1], color[0]);
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
            MessageBox.Show("Reposition");
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

        public void ChangeColor(Color newColor)
        {
            try
            {
                if (CustomColor != newColor)
                {
                    MessageBox.Show(newColor.R.ToString() + ":" + newColor.G.ToString() + ":" + newColor.B.ToString());
                    CustomColor = newColor;
                    SavedColor = newColor;
                    Reposition();
                }
            }
            catch
            {
                //probably not needed but better safe than sorry
            }
        }

        /// <summary>
        /// Check to see if the user inputed keys 0-9, including the numpad keys, but nothing else
        /// </summary>
        /// <param name="e">KeyEventArgs</param>
        private void NumericValidation(KeyEventArgs e)
        {
            //not the most elegant way, but was having issues with the +-./*` keys sneaking in the usual way so took different route
            if (e.Key == Key.D || e.Key == Key.N || e.Key == Key.U || e.Key == Key.M || e.Key == Key.P || e.Key == Key.A)
            {
                e.Handled = true;
            }
            else
            {
                //wanted to have the ability to use numpad too.
                e.Handled = !("D1D2D3D4D5D6D7D8D9D0NumPad0NumPad1NumPad2NumPad3NumPad4NumPad5NumPad6NumPad7NumPad8NumPad9".Contains(e.Key.ToString()));
            }
        }

        /// <summary>
        /// Check to see if the textbox value is over 255. If it is, set it to 255.
        /// </summary>
        /// <param name="sender"></param>
        private void OverNumericValidation(object sender)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            { return; }
            else
            {
                int checkVal = Convert.ToInt32(((TextBox)sender).Text);
                if (checkVal > 255)
                {
                    ((TextBox)sender).Text = "255";
                    ((TextBox)sender).CaretIndex = 3;
                }
            }
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
       
        private void CanColor_LostFocus(object sender, RoutedEventArgs e)
        {
            canFocus = false;
        }

        private void CanColor_GotFocus(object sender, RoutedEventArgs e)
        {
            canFocus = true;
        } 
        
        #endregion

        #region TextBoxes
        private void txtBlue_KeyDown(object sender, KeyEventArgs e)
        {
            NumericValidation(e);
           
        }

        private void txtGreen_KeyDown(object sender, KeyEventArgs e)
        {
            NumericValidation(e);
        }

        private void txtRed_KeyDown(object sender, KeyEventArgs e)
        {
            NumericValidation(e);
        }

        private void txtRed_TextChanged(object sender, TextChangedEventArgs e)
        {
            OverNumericValidation(sender);
        }

        private void txtGreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            OverNumericValidation(sender);
        }

        private void txtBlue_TextChanged(object sender, TextChangedEventArgs e)
        {
            OverNumericValidation(sender);
        }
        #endregion
    }
}
