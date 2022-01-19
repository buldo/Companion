using System.IO.Pipes;
using Companion.Sender.Abstractions;
using JetBrains.Annotations;

namespace Companion.Sender.Pipe;

[PublicAPI]
public class PipeSender : ISender
{
    private readonly NamedPipeServerStream _pipeServerStream =
        new("Companion.Screen", PipeDirection.Out, 1, PipeTransmissionMode.Message);

    public void SendBitmap(IBitmap bitmap)
    {
        _pipeServerStream.Write(bitmap.GetBits());
    }
}