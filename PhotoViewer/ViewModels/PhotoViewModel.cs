using PhotoViewer.Models;
using System;
using System.Windows.Media.Imaging;

namespace PhotoViewer.ViewModels
{
    public class PhotoViewModel : ViewModelBase
    {
        private readonly Photo _photo;
        private BitmapImage? _thumbnail;

        public PhotoViewModel(Models.Photo photo)
        {
            _photo = photo;
        }

        public Models.Photo Photo => _photo;

        public BitmapImage Image
        {
            get
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
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

        public override string ToString() => Photo.ToString();
    }
}
