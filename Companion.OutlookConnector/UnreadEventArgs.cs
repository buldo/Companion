namespace Companion.OutlookConnector;

public class UnreadEventArgs : EventArgs
{
    public UnreadEventArgs(int count)
    {
        Count = count;
    }

    public int Count { get; }
}