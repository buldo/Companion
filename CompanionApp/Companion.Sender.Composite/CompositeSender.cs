using Companion.Sender.Abstractions;
using Companion.Sender.Pipe;
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
    }

    public void SendBitmap(IBitmap bitmap)
    {
        _realSender?.SendBitmap(bitmap);
    }
}