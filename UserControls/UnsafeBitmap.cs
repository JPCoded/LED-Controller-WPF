using System;
using System.Drawing.Imaging;
using System.Windows;


namespace WPF_LED_Controller.UserControls
{
    unsafe class UnsafeBitmap
    {
        System.Drawing.Bitmap bitmap;
        int _width = 201;
        BitmapData _bitmapData = null;
        Byte* _pBase = null;

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

            _width = (int)boundsF.Width * sizeof(PixelData);
            if (_width % 4 != 0)
            {
                _width = 4 * (_width / 4 + 1);
            }
            _bitmapData = bitmap.LockBits(bounds, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            _pBase = (Byte*)_bitmapData.Scan0.ToPointer();
        }

        public PixelData GetPixel(int x, int y)
        {
            PixelData returnValue = *PixelAt(x, y);
            return returnValue;
        }

        private PixelData* PixelAt(int x, int y)
        {
            return (PixelData*)(_pBase + y * _width + x * sizeof(PixelData));
        }

        public void UnlockBitmap()
        {
            bitmap.UnlockBits(_bitmapData);
            _bitmapData = null;
            _pBase = null;
        }
    }
    public struct PixelData
    {
        public byte blue;
        public byte green;
        public byte red;
    }
}
