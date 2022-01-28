using JetBrains.Annotations;

namespace Companion.Sender.Abstractions;

[PublicAPI]
public interface ISender
{
    void SendBitmap(IBitmap bitmap);
}