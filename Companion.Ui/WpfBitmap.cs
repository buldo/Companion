using System.Collections;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Companion.Sender.Abstractions;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Size = System.Windows.Size;

namespace Companion.Ui;

internal class WpfBitmap : IBitmap
{
    private readonly int _width;
    private readonly int _height;
    private byte[] _rendered;

    public WpfBitmap(int width, int height)
    {
        _width = width;
        _height = height;
        Bitmap = new RenderTargetBitmap(_width, _height, 96, 96, PixelFormats.Default);
    }

    public RenderTargetBitmap Bitmap { get; }

    public void ResizeTo(uint width, uint height)
    {

    }

    public byte[] GetBits()
    {
        return _rendered;
    }

    private byte[] GetBitsInner()
    {
        var bwBitmap = new FormatConvertedBitmap(
            Bitmap,
            PixelFormats.BlackWhite,
            BitmapPalettes.BlackAndWhite,
            0);
        var rotBitmap = new TransformedBitmap(bwBitmap, new RotateTransform(90));

        var image = new byte[_width * _height / 8];
        rotBitmap.CopyPixels(image, _height/8, 0);

        return image;
    }

    public void Render(UIElement element)
    {
        var control = new Viewbox();
        control.Child = element;
        control.Measure(new Size(296, 128));
        control.Arrange(new Rect(new Size(296, 128)));
        control.UpdateLayout();
        Bitmap.Render(control);
        _rendered = GetBitsInner();
    }
}