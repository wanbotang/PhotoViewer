using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace PhotoViewer.ViewModels
{
    /// <summary>
    /// A <see cref="ObservableCollection{T}"/> that supports asynchronous operations.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <seealso cref="https://thomaslevesque.com/2009/04/17/wpf-binding-to-an-asynchronous-collection/"/>
    public class AsyncObservableCollection<T> : ObservableCollection<T> where T : class
    {
        private readonly SynchronizationContext? _syncContext = SynchronizationContext.Current;

        private int _position = -1;

        public int Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value < 0 || value > Count - 1 ? -1 : value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Current));
                }
            }
        }

        public T? Current
            => _position >= 0 && _position < Count ? Items[_position] : null;


        public void MoveFirst()
        {
            if (Count > 0) Position = 0;
        }

        public void MoveLast()
        {
            if (Count > 0) Position = Count - 1;
        }

        public void MoveNext()
        {
            if (Position < Count - 1) Position++;
        }

        public void MovePrevious()
        {
            if (Position > 0) Position--;
        }

        public int FindIndex(Predicate<T> match) => FindIndex(0, match);

        public int FindIndex(int startIndex, Predicate<T> match)
        {
            if (startIndex >= 0 && startIndex < Count)
            {
                for (int index = startIndex; index < Count; index++)
                {
                    if (match(Items[index]))
                    {
                        return index;
                    }
                }
            }

            return -1;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (_syncContext == null || SynchronizationContext.Current == _syncContext)
            {
                base.OnCollectionChanged(e);
            }
            else
            {
                _syncContext.Send(RaiseCollectionChangedEvent, e);
            }
        }

        private void RaiseCollectionChangedEvent(object? eventArgs)
        {
            if (eventArgs is NotifyCollectionChangedEventArgs e)
            {
                base.OnCollectionChanged(e);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (_syncContext == null || SynchronizationContext.Current == _syncContext)
            {
                base.OnPropertyChanged(e);
            }
            else
            {
                _syncContext.Send(RaisePropertyChangedEvent, e);
            }
        }

        private void RaisePropertyChangedEvent(object? eventArgs)
        {
            if (eventArgs is PropertyChangedEventArgs e)
            {
                base.OnPropertyChanged(e);
            }
        }
    }
}
