using PhotoViewer.Commands;
using PhotoViewer.Models;
using PhotoViewer.Services;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PhotoViewer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly object _syncLock = new();

        private readonly string? _path;
        private string? _photosFolderPath;
        private IPageViewModel? _currentPageViewModel;

        public MainWindowViewModel(IPhotoService photoService, string? path = null)
        {
            PhotoService = photoService;
            Photos = new PhotoViewModelCollection();
            BindingOperations.EnableCollectionSynchronization(Photos, _syncLock);

            // The path is from the command line arguments. 
            if (path != null)
            {
                if (File.Exists(path))
                {
                    _path = path;
                    _photosFolderPath = Path.GetDirectoryName(path);
                }
                else if (Directory.Exists(path))
                {
                    _photosFolderPath = path;
                }
            }

            _currentPageViewModel = new PhotosViewModel(Photos);
            LoadPhotosCommand = new AsyncCommand(LoadPhotosAsync);
        }

        public IPhotoService PhotoService { get; }

        public AsyncCommand LoadPhotosCommand { get; }
        public PhotoViewModelCollection Photos { get; }

        public string PhotosFolderPath
            => _photosFolderPath ?? Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        public IPageViewModel? CurrentPageViewModel
        {
            get => _currentPageViewModel;
            set
            {
                _currentPageViewModel = value;
                OnPropertyChanged();
            }
        }

        private Task LoadPhotosAsync(CancellationToken? cancellationToken = null)
        {
            Photos.Clear();

            return Task.Run(() =>
            {
                Photo? current = null;
                if (_path != null)
                {
                    current = PhotoService.GetPhoto(_path);
                    if (current != null)
                    {
                        Photos.Current = new PhotoViewModel(current);
                    }
                }

                int index = -1;
                foreach (var photo in PhotoService.GetPhotos(PhotosFolderPath))
                {
                    index++;
                    var pv = new PhotoViewModel(photo);
                    lock (_syncLock) { Photos.Add(pv); }

                    if (Photos.Current == null)
                    {
                        Photos.Position = 0;
                    }
                    else if (current != null)
                    {
                        if (photo.UriSource.Equals(current.UriSource))
                        {
                            Photos.Select(index);
                        }
                    }
                }
            });
        }
    }
}
