using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Whip.Common.Model
{
    public class Track : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isCurrentTrack;

        public Track()
        {
            Tags = new List<string>();
        }

        public File File { get; set; }
        public string Title { get; set; }
        public int TrackNo { get; set; }
        public TimeSpan Duration { get; set; }
        public List<string> Tags { get; set; }
        public string Year { get; set; }
        public string Lyrics { get; set; }
        public bool Instrumental { get; set; }

        public Artist Artist { get; set; }
        public Disc Disc { get; set; }

        public bool IsCurrentTrack
        {
            get { return _isCurrentTrack; }
            set
            {
                if (_isCurrentTrack == value)
                    return;

                _isCurrentTrack = value;
                OnPropertyChanged(nameof(IsCurrentTrack));
            }
        }

        public string TagsDescription => Tags.Any() ? string.Join(", ", Tags) : "-";
        public string TrackNoDescription => string.Format("{0} of {1}", TrackNo, Disc.TrackCount);
        public string DiscNoDescription => string.Format("{0} of {1}", Disc.DiscNo, Disc.Album.DiscCount);

        public override string ToString()
        {
            return $"{Title} by {Artist.Name}";
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
