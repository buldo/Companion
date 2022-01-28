using System.IO.Pipes;
using Companion.Sender.Abstractions;
using JetBrains.Annotations;

namespace Companion.Sender.Pipe;

[PublicAPI]
public class PipeSender : ISender
{
    private readonly NamedPipeServerStream _pipeServerStream =
        new("Companion.Screen", PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.WriteThrough,0,0);

    public PipeSender()
    {
        _pipeServerStream.WaitForConnection();
    }

    public void SendBitmap(IBitmap bitmap)
    {
        if (_pipeServerStream.IsConnected)
        {
            _pipeServerStream.Write(bitmap.GetBits());
        }
    }
}