using PhotoViewer.Services;
using System;
using System.IO;

namespace PhotoViewer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string? _photosFolderPath;
        private IPageViewModel? _currentPageViewModel;

        public MainWindowViewModel(IPhotoService photoService, string? path = null)
        {
            PhotoService = photoService;
            Photos = new AsyncObservableCollection<PhotoViewModel>();

            // The file name is from the command line arguments. 
            string? fileName = null;
            if (path != null)
            {
                if (File.Exists(path))
                {
                    fileName = path;
                    _photosFolderPath = Path.GetDirectoryName(path);
                }
                else if (Directory.Exists(path))
                {
                    _photosFolderPath = path;
                }
            }

            LoadPhotos();
            if (fileName != null)
            {
                Uri uri = new Uri(fileName);
                Photos.Position = Photos.FindIndex(p => p.Photo.UriSource == uri);
            }
            else
            {
                Photos.MoveFirst();
            }
            _currentPageViewModel = new PhotosViewModel(Photos);
        }

        public IPhotoService PhotoService { get; }
        public AsyncObservableCollection<PhotoViewModel> Photos { get; }

        public string PhotosFolderPath
        {
            get => _photosFolderPath ?? Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            set
            {
                _photosFolderPath = value;
                OnPropertyChanged();

                // Reload the photos when the path of folder changed.
                LoadPhotos();
            }
        }

        public IPageViewModel? CurrentPageViewModel
        {
            get => _currentPageViewModel;
            set
            {
                _currentPageViewModel = value;
                OnPropertyChanged();
            }
        }

        private void LoadPhotos()
        {
            Photos.Clear();
            foreach (var photo in PhotoService.GetPhotos(PhotosFolderPath))
            {
                Photos.Add(new PhotoViewModel(photo));
            }
        }
    }
}
