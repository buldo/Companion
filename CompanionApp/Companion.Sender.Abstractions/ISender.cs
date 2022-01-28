using JetBrains.Annotations;

namespace Companion.Sender.Abstractions;

[PublicAPI]
public interface ISender
{
    Task SendBitmapAsync(IBitmap bitmap);
}