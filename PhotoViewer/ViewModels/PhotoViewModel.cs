using PhotoViewer.Models;
using System;
using System.Windows.Media.Imaging;

namespace PhotoViewer.ViewModels
{
    public class PhotoViewModel : ViewModelBase
    {
        private readonly Photo _photo;

        private double _imageWidth;
        private double _imageHeight;
        private BitmapImage? _thumbnail;

        public PhotoViewModel(Photo photo)
        {
            _photo = photo;
            Initialize();
        }

        public Photo Photo => _photo;

        public double ImageWidth => _imageWidth;
        public double ImageHeight => _imageHeight;

        public BitmapImage Image
        {
            get
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                // Optimize memory, do not use cache.
                image.CacheOption = BitmapCacheOption.None;
                image.UriSource = _photo.UriSource;
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
                    _thumbnail = CreateThumbnail(_photo.UriSource);
                }

                return _thumbnail;
            }
            set
            {
                _thumbnail = value;
                OnPropertyChanged();
            }
        }

        private void Initialize()
        {
            // Get the size of the image.
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.None;
            image.CreateOptions = BitmapCreateOptions.DelayCreation;
            image.UriSource = _photo.UriSource;
            image.EndInit();
            _imageWidth = image.Width;
            _imageHeight = image.Height;

            // Create thumbnail.
            _thumbnail = CreateThumbnail(_photo.UriSource);
        }

        public static BitmapImage CreateThumbnail(Uri uriSource, int pixelHeight = 64)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
            image.UriSource = uriSource;
            image.DecodePixelHeight = pixelHeight;
            image.EndInit();
            image.Freeze();

            return image;
        }

        public override string ToString() => Photo.ToString();
    }
}
