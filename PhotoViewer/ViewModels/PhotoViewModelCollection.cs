using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PhotoViewer.ViewModels
{
    public class PhotoViewModelCollection : ObservableCollection<PhotoViewModel>
    {
        private int _position = -1;
        private PhotoViewModel? _current;

        public PhotoViewModelCollection()
        { }

        public int Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value < 0 || value > Count - 1 ? -1 : value;
                    OnPropertyChanged();

                    Current = _position == -1 ? null : Items[_position];
                }
            }
        }

        public PhotoViewModel? Current
        {
            get => _current;
            set
            {
                _current = value;
                OnPropertyChanged();
            }
        }

        internal void Select(int index)
        {
            _position = index;
            OnPropertyChanged(nameof(Position));
        }

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

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
