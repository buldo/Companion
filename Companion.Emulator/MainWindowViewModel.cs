using System;
using System.Windows;
using System.Windows.Media;
using Prism.Mvvm;
using System.Windows.Media.Imaging;

namespace Companion.Emulator
{
    internal class MainWindowViewModel : BindableBase
    {
        //private const int Width = 296;
        //private const int Height = 128;
        private const int Height = 296;
        private const int Width = 128;
        private readonly WriteableBitmap _frameBuffer = new(Width, Height, 96, 96, PixelFormats.BlackWhite, BitmapPalettes.BlackAndWhite);
        private readonly Int32Rect _writeRect = new(0, 0, Width, Height);

        public WriteableBitmap FrameBuffer => _frameBuffer;

        public void SetFrameBuffer(byte[] buffer)
        {
            FrameBuffer.WritePixels(_writeRect, buffer, Width/8, 0);
        }
    }
}
