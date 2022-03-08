using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace PhotoViewer
{
    public class PhotoViewModel : INotifyPropertyChanged
    {
        private readonly Uri _uriSource;
        private string? _name;
        private BitmapImage? _thumbnail;

        public PhotoViewModel(Uri uriSource)
        {
            _uriSource = uriSource;
        }

        public Uri UriSource => _uriSource;

        public string Name
        {
            get => _name ?? _uriSource.ToString();
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public BitmapImage Image
        {
            get
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = _uriSource;
                image.EndInit();
                image.Freeze();

                return image;
            }
        }

        public BitmapImage? Thumbnail
        {
            get
            {
                if (_thumbnail == null)
                {
                    _thumbnail = CreateThumbnail(_uriSource);
                }

                return _thumbnail;
            }
        }

        public static BitmapImage CreateThumbnail(Uri uriSource, int pixelHeight = 64)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.DelayCreation;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = uriSource;
            image.DecodePixelHeight = pixelHeight;
            image.EndInit();
            image.Freeze();

            return image;
        }

        public override string ToString() => Name;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
