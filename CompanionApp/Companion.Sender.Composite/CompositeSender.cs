using Companion.Sender.Abstractions;
using Companion.Sender.Pipe;
using Companion.Sender.Usb;

using JetBrains.Annotations;

namespace Companion.Sender.Composite;

[PublicAPI]
public class CompositeSender : ISender
{
    private ISender? _realSender;

    public void OpenConnection(string connectionString)
    {
        if (string.Equals(connectionString, "pipe"))
        {
            _realSender = new PipeSender();
        }
        if (string.Equals(connectionString, "usb"))
        {
            _realSender = new UsbSender();
        }
    }

    public async Task SendBitmapAsync(IBitmap bitmap)
    {
        if(_realSender != null)
        {
            await _realSender.SendBitmapAsync(bitmap).ConfigureAwait(false);
        }
    }
}