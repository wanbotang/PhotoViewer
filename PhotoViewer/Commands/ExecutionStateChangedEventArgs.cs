using System;

namespace PhotoViewer.Commands
{
    public class ExecutionStateChangedEventArgs : EventArgs
    {
        public ExecutionStateChangedEventArgs(bool isExecuting)
        {
            IsExecuting = isExecuting;
        }

        public bool IsExecuting { get; }
    }
}
