using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhotoViewer.Commands
{
    public class AsyncCommand : IAsyncCommand
    {
        private readonly Func<CancellationToken?, Task> _execute;
        private readonly Func<bool>? _canExecute;

        private CancellationTokenSource? _cancellationTokenSource;
        private ICommand? _cancelCommand;
        private bool _isExecuting;

        public AsyncCommand(Func<CancellationToken?, Task> execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler<ExecutionStateChangedEventArgs>? ExecutionStateChanged;

        public bool IsExecuting => _isExecuting;

        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(
                        () => _cancellationTokenSource?.Cancel(),
                        () => !_isExecuting);
                }

                return _cancelCommand;
            }
        }

        protected virtual void OnExecutionStateChanged(bool isExecuting)
        {
            _isExecuting = isExecuting;
            ExecutionStateChanged?.Invoke(this, new ExecutionStateChangedEventArgs(isExecuting));
            CommandManager.InvalidateRequerySuggested();
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
            => !_isExecuting && (_canExecute == null || _canExecute.Invoke());

        public async void Execute(object? parameter)
        {
            if (_isExecuting) { return; }

            // Start executing command.
            _cancellationTokenSource = new CancellationTokenSource();
            OnExecutionStateChanged(isExecuting: true);
            try
            {
                await _execute(_cancellationTokenSource.Token);
            }
            finally
            {
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;

                // Command executed.
                OnExecutionStateChanged(isExecuting: false);
            }
        }
    }
}
