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
using System.Drawing.Imaging;

namespace WPF_LED_Controller
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// Added my RGBBox control into this one. While it makes this code much longer because of added control, it makes it so I don't have to keep making calls from 2 different controls that are to be used by each other so often. Needs huge rework later to help with changing color and text boxes. At the moment it's not very effecient or well designed but will get to it later.
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        private Color _customColor = Colors.Transparent;
        private UnsafeBitmap myUnsafeBitmap;
        public event EventHandler<EventArgs> TextChanged;

        /// <summary>
        /// Not my code. The Unsafe bitmap idea was taken from MSDN article. Just had to use the full names for things such as System.Drawing.Bitmap, because System.Drawing has a Color funtion that messes with the WPF color function. Also modified it a slight bit for my needs and removed unneeded items. Also made few functions that were once public private.
        /// </summary>
        private unsafe class UnsafeBitmap
        {
            System.Drawing.Bitmap bitmap;
            int width = 201;
            BitmapData bitmapData = null;
            Byte* pBase = null;

            public UnsafeBitmap(System.Drawing.Bitmap bitmap)
            {
                this.bitmap = new System.Drawing.Bitmap(bitmap);
            }

            public void Dispose()
            {
                bitmap.Dispose();
            }

            public System.Drawing.Bitmap Bitmap
            {
                get { return (bitmap); }
            }

            private Point PixelSize
            {
                get
                {
                    System.Drawing.GraphicsUnit unit = System.Drawing.GraphicsUnit.Pixel;
                    System.Drawing.RectangleF bounds = bitmap.GetBounds(ref unit);
                    return new Point((int)bounds.Width, (int)bounds.Height);
                }
            }
            public void LockBitmap()
            {
                System.Drawing.GraphicsUnit unit = System.Drawing.GraphicsUnit.Pixel;
                System.Drawing.RectangleF boundsF = bitmap.GetBounds(ref unit);
                System.Drawing.Rectangle bounds = new System.Drawing.Rectangle((int)boundsF.X, (int)boundsF.Y, (int)boundsF.Width, (int)boundsF.Height);

                width = (int)boundsF.Width * sizeof(PixelData);
                if (width % 4 != 0)
                {
                    width = 4 * (width / 4 + 1);
                }
                bitmapData = bitmap.LockBits(bounds, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                pBase = (Byte*)bitmapData.Scan0.ToPointer();
            }

            public PixelData GetPixel(int x, int y)
            {
                PixelData returnValue = *PixelAt(x, y);
                return returnValue;
            }

            private PixelData* PixelAt(int x, int y)
            {
                return (PixelData*)(pBase + y * width + x * sizeof(PixelData));
            }

            public void UnlockBitmap()
            {
                bitmap.UnlockBits(bitmapData);
                bitmapData = null;
                pBase = null;
            }

        }
        public struct PixelData
        {
            public byte blue;
            public byte green;
            public byte red;
        }

        public Color CustomColor
        {
            get
            {
                return _customColor;
            }
            private set
            {
                if (_customColor != value)
                {
                    _customColor = value;
                    MadHatter();
                }
            }
        }

        /// <summary>
        /// Called when ever customColor is changed. Changes the values of the text box, like mad hatter saying "Change places!"
        /// </summary>
        public void MadHatter()
        {
            txtRed.Text = CustomColor.R.ToString();
           
            txtRHex.Text = CustomColor.R.ToString();
           // txtRHex.Text = CustomColor.R.ToString("X").PadLeft(2, '0');
            txtGreen.Text = CustomColor.G.ToString();
            txtGHex.Text = CustomColor.G.ToString("X").PadLeft(2, '0');
            txtBlue.Text = CustomColor.B.ToString();
            txtBHex.Text = CustomColor.B.ToString("X").PadLeft(2, '0');
            txtHAll.Text = String.Format("#{0}{1}{2}", txtRHex.Text, txtGHex.Text, txtBHex.Text);
        }

        /// <summary>
        /// Color that the mouse is current hovered over.
        /// </summary>
        public Color HoverColor { get; private set; }
        public ColorPicker()
        {
            InitializeComponent();
            myUnsafeBitmap = new UnsafeBitmap(WPF_LED_Controller.Properties.Resources.ColorSwatch);
        }

        #region Custom Functions
        private Color GetColorFromImage(int i, int j)
        {
            myUnsafeBitmap.LockBitmap();
            PixelData pixel = myUnsafeBitmap.GetPixel(i, j);
            Color Colorfromimagepoint = Color.FromRgb(pixel.red, pixel.green, pixel.blue);
            myUnsafeBitmap.UnlockBitmap();
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
            myUnsafeBitmap.LockBitmap();

            for (int i = 0; i < CanColor.ActualWidth; i++)
            {
                bool flag = false;
                for (int j = 0; j < CanColor.ActualHeight; j++)
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
                CustomColor = GetColorFromImage((int)Mouse.GetPosition(CanColor).X, (int)Mouse.GetPosition(CanColor).Y);
                MovePointer();
            }
            catch
            {
                return;
            }
        }

        private void ChangeColor(Color newColor)
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

        //need to rework most of the validation functions to reduce code and make everything less confusing.
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

        private void HexValidation(KeyEventArgs e)
        {
            //for some reason, this way works here, but won't work for the NumericValidation. 
            string input = e.Key.ToString();
            if (e.Key == Key.D3 && (e.Key == Key.LeftShift || e.Key == Key.RightShift))
            { input = "#"; }
            if (!(input == "#" || (input[0] >= 'A' && input[0] <= 'F') || (input[0] >= 'a' && input[0] <= 'F') || (input[0] >= '0' && input[0] <= '9')))
            {
                e.Handled = true;
            }

        }
        private string OverUnderValidation(string ValueToCheck)
        {
            if (!string.IsNullOrEmpty(ValueToCheck))
            {
                int checkVal = Convert.ToInt32(ValueToCheck);
                if (checkVal > 255)
                { return "255"; }
                else if (checkVal < 0)
                { return "0"; }
            }

            return ValueToCheck;
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

        #region TextBoxes
        private void txtBlue_KeyDown(object sender, KeyEventArgs e)
        {
            NumericValidation(e);
            string bsValue = string.IsNullOrEmpty(((TextBox)sender).Text) ? "0" : ((TextBox)sender).Text;
            byte bbyteValue = Convert.ToByte(bsValue);
            ChangeColor(Color.FromRgb(CustomColor.R, CustomColor.G, bbyteValue));
        }

        private void txtGreen_KeyDown(object sender, KeyEventArgs e)
        {
            NumericValidation(e);
            string gsValue = string.IsNullOrEmpty(((TextBox)sender).Text) ? "0" : ((TextBox)sender).Text;
            byte gbyteValue = Convert.ToByte(gsValue);
            ChangeColor(Color.FromRgb(CustomColor.R, gbyteValue, CustomColor.B));
        }

        private void txtRed_KeyDown(object sender, KeyEventArgs e)
        {
            NumericValidation(e);
            string rsValue = string.IsNullOrEmpty(((TextBox)sender).Text) ? "0" : ((TextBox)sender).Text;
            byte rbyteValue = Convert.ToByte(rsValue);
            ChangeColor(Color.FromRgb(rbyteValue, CustomColor.G, CustomColor.B));
        }

        private void txtRed_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text);

            if (((TextBox)sender).Text != CustomColor.R.ToString())
            {
                byte rbyteValue = Convert.ToByte(((TextBox)sender).Text);
                ChangeColor(Color.FromRgb(rbyteValue, CustomColor.G, CustomColor.B));
            }
            if (TextChanged != null)
            {
                TextChanged(this, EventArgs.Empty);
            }
        }

        private void txtGreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text);
            if (((TextBox)sender).Text != CustomColor.R.ToString())
            {
                byte gbyteValue = Convert.ToByte(((TextBox)sender).Text);
                ChangeColor(Color.FromRgb(CustomColor.R, gbyteValue, CustomColor.B));
            }
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text);

            if (TextChanged != null)
            {
                TextChanged(this, EventArgs.Empty);
            }
        }

        private void txtBlue_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = OverUnderValidation(((TextBox)sender).Text);


            if (((TextBox)sender).Text != CustomColor.R.ToString())
            {
                byte bbyteValue = Convert.ToByte(((TextBox)sender).Text);
                ChangeColor(Color.FromRgb(CustomColor.R, CustomColor.G, bbyteValue));
            }
            if (TextChanged != null)
            {
                TextChanged(this, EventArgs.Empty);
            }
        }


        private void txtRed_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                int newValue = Convert.ToInt32(((TextBox)sender).Text) + 1;
                ((TextBox)sender).Text = newValue.ToString();
            }
            else if (e.Key == Key.Down)
            {
                int newValue = Convert.ToInt32(((TextBox)sender).Text) - 1;
                ((TextBox)sender).Text = newValue.ToString();
            }
        }

        private void txtGreen_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                int newValue = Convert.ToInt32(((TextBox)sender).Text) + 1;
                ((TextBox)sender).Text = newValue.ToString();
            }
            else if (e.Key == Key.Down)
            {
                int newValue = Convert.ToInt32(((TextBox)sender).Text) - 1;
                ((TextBox)sender).Text = newValue.ToString();
            }
        }

        private void txtBlue_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                int newValue = Convert.ToInt32(((TextBox)sender).Text) + 1;
                ((TextBox)sender).Text = newValue.ToString();
            }
            else if (e.Key == Key.Down)
            {
                int newValue = Convert.ToInt32(((TextBox)sender).Text) - 1;
                ((TextBox)sender).Text = newValue.ToString();
            }
        }

        private void txtHAll_KeyDown(object sender, KeyEventArgs e)
        {
            HexValidation(e);
            try
            {
                MessageBox.Show(ColorConverter.ConvertFromString(txtHAll.Text).ToString());
                CustomColor = (Color)ColorConverter.ConvertFromString(txtHAll.Text); }
            catch 
            { }
        }

        private void txtRHex_KeyDown(object sender, KeyEventArgs e)
        {
            HexValidation(e);
        }

        private void txtGHex_KeyDown(object sender, KeyEventArgs e)
        {
            HexValidation(e);
        }

        private void txtBHex_KeyDown(object sender, KeyEventArgs e)
        {
            HexValidation(e);
        }
        #endregion
    }
}
