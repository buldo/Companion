using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;

namespace Companion.Ui
{
    internal class MainWindowViewModel : BindableBase
    {
        private string? _connectionPath;

        public MainWindowViewModel()
        {
            StartCommand = new DelegateCommand(ExecuteConnect, CanExecuteConnect);
        }

        public string? ConnectionPath
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

        public DelegateCommand StartCommand { get; }

        private bool CanExecuteConnect()
        {
            return !string.IsNullOrWhiteSpace(ConnectionPath) &&
                   (ConnectionPath == "pipe" || ConnectionPath.StartsWith("COM"));
        }

        private void ExecuteConnect()
        {

        }
    }
}
