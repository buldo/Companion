using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Companion.Sender.Abstractions;

namespace Companion.Ui
{
    internal class WpfBitmap : IBitmap
    {
        private readonly int _width;
        private readonly int _height;

        public WpfBitmap(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public WriteableBitmap Bitmap =
            new(296, 128, 96, 96, PixelFormats.BlackWhite, BitmapPalettes.BlackAndWhite);

        public void ResizeTo(uint width, uint height)
        {

        }

        public byte[] GetBits()
        {
            var image = new byte[_width * _height / 8];
            Bitmap.CopyPixels(new Int32Rect(0, 0, 296, 128), image, _width / 8, 0);

            var ret = new byte[image.Length];
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    ret[i * _height + j] = image[j * _width + i];
                }
            }

            return ret;
        }
    }
}
