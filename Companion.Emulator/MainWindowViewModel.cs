using System;
using System.IO.Pipes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Prism.Mvvm;
using System.Windows.Media.Imaging;
using System.Security.Principal;

namespace Companion.Emulator
{
    internal class MainWindowViewModel : BindableBase
    {
        //private const int Width = 296;
        //private const int Height = 128;
        private const int Height = 296;
        private const int Width = 128;
        private readonly Int32Rect _writeRect = new(0, 0, Width, Height);
        private readonly NamedPipeClientStream _pipe = new ("Companion.Screen");

        public WriteableBitmap FrameBuffer { get; } = new(Width, Height, 96, 96, PixelFormats.BlackWhite, BitmapPalettes.BlackAndWhite);

        private Task _readTask;

        public void Start()
        {
            _readTask = Task.Run(Read);
        }

        private void Read()
        {
            _pipe.Connect();
            var buffer = new byte[Width*Height/8];
            while (true)
            {
                _pipe.Read(buffer, 0, buffer.Length);
                SetFrameBuffer(buffer);
            }
        }

        public void SetFrameBuffer(byte[] buffer)
        {
            Application.Current.Dispatcher.Invoke(() => FrameBuffer.WritePixels(_writeRect, buffer, Width / 8, 0));
        }
    }
}
