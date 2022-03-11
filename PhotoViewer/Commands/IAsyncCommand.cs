using System;
using System.Windows.Input;

namespace PhotoViewer.Commands
{
    public interface IAsyncCommand : ICommand
    {
        event EventHandler<ExecutionStateChangedEventArgs>? ExecutionStateChanged;
        ICommand CancelCommand { get; }
    }
}
