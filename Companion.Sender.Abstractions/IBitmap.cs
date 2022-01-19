using JetBrains.Annotations;

namespace Companion.Sender.Abstractions;

[PublicAPI]
public interface IBitmap
{
    public void ResizeTo(uint width, uint height);

    public byte[] GetBits();
}