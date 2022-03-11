using System.Collections.Generic;

namespace PhotoViewer.Services
{
    public interface IPhotoService
    {
        public Models.Photo? GetPhoto(string uri);
        public IEnumerable<Models.Photo> GetPhotos(string? location = null);
    }
}
