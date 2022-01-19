using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Companion.OutlookConnector;
using Prism.Commands;
using Prism.Mvvm;

namespace Companion.Ui
{
    internal class MainWindowViewModel : BindableBase
    {
        private readonly OutlookMonitor _monitor;
        private string _connectionPath = string.Empty;
        private int _unread = 0;

        public MainWindowViewModel()
        {
            StartCommand = new DelegateCommand(ExecuteConnect, CanExecuteConnect);
            _monitor = new OutlookMonitor();
            _monitor.Unread += MonitorOnUnread;
        }

        public string ConnectionPath
        {
            get => _connectionPath;
            set
            {
                if (SetProperty(ref _connectionPath, value))
                {
                    StartCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public int Unread
        {
            get => _unread;
            set => SetProperty(ref _unread, value);
        }

        public DelegateCommand StartCommand { get; }

        private bool CanExecuteConnect()
        {
            return !string.IsNullOrWhiteSpace(ConnectionPath) &&
                   (ConnectionPath == "pipe" || ConnectionPath.StartsWith("COM"));
        }

        private void ExecuteConnect()
        {
            _monitor.StartListen();
        }

        private void MonitorOnUnread(object? sender, UnreadEventArgs e)
        {
            Unread = e.Count;
        }
    }
}
