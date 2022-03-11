using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoViewer.Services
{
    public class LocalPhotoService : IPhotoService
    {
        public static readonly string[] SupportedImageFileExtensions =
{
            ".jpg",
            ".jpeg",
            ".png",
            ".bmp"
        };

        public Models.Photo? GetPhoto(string path)
        {
            var file = new FileInfo(path);
            if (!file.Exists) { return null; }
            return new Models.Photo(new Uri(file.FullName, UriKind.Absolute))
            {
                Name = file.Name
            };
        }

        public IEnumerable<Models.Photo> GetPhotos(string? location = null)
        {
            if (location == null)
            {
                location = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }

            var directory = new DirectoryInfo(location);
            if (!directory.Exists)
            {
                yield break;
            }

            foreach (var file in directory.EnumerateFiles())
            {
                if (SupportedImageFileExtensions.Contains(file.Extension, StringComparer.OrdinalIgnoreCase))
                {
                    yield return new Models.Photo(new Uri(file.FullName, UriKind.Absolute))
                    {
                        Name = file.Name
                    };
                }
            }
        }
    }
}
