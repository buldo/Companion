using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Companion.OutlookConnector;
using Companion.Sender.Composite;
using Prism.Commands;
using Prism.Mvvm;

namespace Companion.Ui
{
    internal class MainWindowViewModel : BindableBase
    {
        private readonly CompositeSender _sender = new();
        private readonly OutlookMonitor _monitor;
        private string _connectionPath = string.Empty;
        private int _unread;

        public MainWindowViewModel()
        {
            StartCommand = new DelegateCommand(ExecuteConnect, CanExecuteConnect);
            _monitor = new OutlookMonitor();
            _monitor.Unread += MonitorOnUnreadAsync;
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
            _sender.OpenConnection(ConnectionPath);
            _monitor.StartListen();
        }

        private async void MonitorOnUnreadAsync(object? sender, UnreadEventArgs e)
        {
            try
            {
                Unread = e.Count;

                WpfBitmap bitmap = null;
                Application.Current.Dispatcher.Invoke(() => {
                    bitmap = new WpfBitmap(296, 128);
                    var element = new NotificationPreview() { DataContext = this };
                    bitmap.Render(element);
                });

                await _sender.SendBitmapAsync(bitmap);
            }
            catch (Exception)
            {
            }
        }
    }
}
