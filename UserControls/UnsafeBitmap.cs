using System;
using System.Drawing;
using System.Drawing.Imaging;



namespace WPF_LED_Controller.UserControls
{
    unsafe class UnsafeBitmap
    {
        readonly Bitmap _bitmap;
        int _width = 201;
        BitmapData _bitmapData;
        Byte* _pBase = null;

        public UnsafeBitmap(Bitmap bitmap)
        {
            _bitmap = new Bitmap(bitmap);
        }

        public void Dispose()
        {
            _bitmap.Dispose();
        }

        public Bitmap Bitmap
        {
            get { return (_bitmap); }
        }

        public void LockBitmap()
        {
            var unit = GraphicsUnit.Pixel;
            RectangleF boundsF = _bitmap.GetBounds(ref unit);
            var bounds = new Rectangle((int)boundsF.X, (int)boundsF.Y, (int)boundsF.Width, (int)boundsF.Height);

            _width = (int)boundsF.Width * sizeof(PixelData);
            if (_width % 4 != 0)
            {
                _width = 4 * (_width / 4 + 1);
            }
            _bitmapData = _bitmap.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
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
            _bitmap.UnlockBits(_bitmapData);
            _bitmapData = null;
            _pBase = null;
        }
    }
    public struct PixelData
    {
        public byte Blue;
        public byte Green;
        public byte Red;
    }
}
