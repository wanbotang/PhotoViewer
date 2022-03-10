using PhotoViewer.Commands;

namespace PhotoViewer.ViewModels
{
    public class PhotosViewModel : ViewModelBase, IPageViewModel
    {
        private bool _isPhotoListVisible = true;

        public PhotosViewModel(AsyncObservableCollection<PhotoViewModel> photos)
        {
            Photos = photos;
            MovePreviousCommand = new RelayCommand(
                execute: () => Photos.MovePrevious(),
                canExecute: () => Photos.Position > 0);
            MoveNextCommand = new RelayCommand(
                execute: () => Photos.MoveNext(),
                canExecute: () => Photos.Position < Photos.Count - 1);
            TogglePhotoListVisibleCommand = new RelayCommand(() => IsPhotoListVisible = !IsPhotoListVisible);
        }

        public RelayCommand MovePreviousCommand { get; }
        public RelayCommand MoveNextCommand { get; }
        public RelayCommand TogglePhotoListVisibleCommand { get; }
        public AsyncObservableCollection<PhotoViewModel> Photos { get; }

        public bool IsPhotoListVisible
        {
            get => _isPhotoListVisible;
            set
            {
                _isPhotoListVisible = value;
                OnPropertyChanged();
            }
        }

        public string? Title => Photos.Current?.ToString();
    }
}
