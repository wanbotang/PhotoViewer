using System;

namespace PhotoViewer.Models
{
    public class Photo
    {
        private Uri _uriSource;
        private string? _name;

        public Photo(Uri uriSource)
        {
            _uriSource = uriSource;
        }

        public Uri UriSource => _uriSource;

        public string Name
        {
            get => _name ?? _uriSource.ToString();
            set => _name = value;
        }

        public override string ToString() => Name;
    }
}
