using Microsoft.Office.Interop.Outlook;
using System.Runtime.InteropServices;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Companion.OutlookConnector
{
    public class OutlookMonitor
    {
        private readonly Timer _requestTimer;
        private readonly OutlookProvider _outlookProvider;

        private int _lastUnreadCount = -1;

        public OutlookMonitor()
        {
            _outlookProvider = new OutlookProvider();
            _requestTimer = new Timer(1000){AutoReset = true};
            _requestTimer.Elapsed+= RequestTimerOnElapsed;
        }

        public event EventHandler<UnreadEventArgs>? Unread;

        public void StartListen()
        {
            _requestTimer.Start();
        }

        private void RequestTimerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            var app = _outlookProvider.GetApplication();
            var inbox = app.Session.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
            var unread = inbox.Items.Restrict("[Unread]=true");
            if (unread.Count != _lastUnreadCount)
            {
                _lastUnreadCount = unread.Count;
                Unread?.Invoke(this, new UnreadEventArgs(_lastUnreadCount));
            }
        }
    }
}