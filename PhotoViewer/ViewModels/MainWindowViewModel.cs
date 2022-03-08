using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PhotoViewer
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public static readonly string[] SupportedImageFileExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".bmp"
        };

        private string? _photosFolderPath;
        private PhotoViewModel? _selectedPhoto;

        public MainWindowViewModel() :
            this(null)
        { }

        public MainWindowViewModel(string? photosFolderPath)
        {
            _photosFolderPath = photosFolderPath;
            Load();
        }

        public string PhotosFolderPath
        {
            get => _photosFolderPath ?? Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            set
            {
                _photosFolderPath = value;
                OnPropertyChanged();

                // Reload the photos when the path of folder changed.
                Load();
            }
        }

        public PhotoViewModel? SelectedPhoto
        {
            get => _selectedPhoto;
            set
            {
                _selectedPhoto = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PhotoViewModel> Photos { get; } = new ObservableCollection<PhotoViewModel>();

        protected void Load()
        {
            Photos.Clear();
            SelectedPhoto = null;

            var directory = new DirectoryInfo(PhotosFolderPath);
            if (directory.Exists)
            {
                foreach (var file in directory.GetFiles())
                {
                    if (SupportedImageFileExtensions.Contains(file.Extension, StringComparer.OrdinalIgnoreCase))
                    {
                        var photo = new PhotoViewModel(new Uri(file.FullName))
                        {
                            Name = file.Name
                        };
                        Photos.Add(photo);
                    }
                }

                if (Photos.Count > 0)
                {
                    SelectedPhoto = Photos[0];
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
