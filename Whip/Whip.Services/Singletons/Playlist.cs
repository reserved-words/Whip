using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.ExtensionMethods;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Common.TrackSorters;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Services
{
    public class Playlist : IPlaylist
    {
        private readonly TrackQueue _queue = new TrackQueue();
        private readonly IUserSettings _userSettings;

        public Playlist(IUserSettings userSettings)
        {
            _userSettings = userSettings;
        }

        public event Action ListUpdated;
        public event Action<Track> CurrentTrackChanged;

        public Track CurrentTrack => _queue.CurrentTrack;

        public string PlaylistName { get; private set; }

        public List<Track> Tracks { get; private set; }

        public void Set(string playlistName, List<Track> tracks, Track startAt)
        {
            PlaylistName = playlistName;
            Tracks = tracks;

            var sortedTracks = Sort(tracks);

            _queue.Set(sortedTracks, startAt == null ? 0 : sortedTracks.IndexOf(startAt), true);
            
            ListUpdated?.Invoke();
        }

        public void MoveNext()
        {
            _queue.MoveNext();
            CurrentTrackChanged?.Invoke(_queue.CurrentTrack);
        }

        public void MovePrevious()
        {
            _queue.MovePrevious();
            CurrentTrackChanged?.Invoke(_queue.CurrentTrack);
        }

        private List<Track> Sort(List<Track> tracks)
        {
            var sorter = _userSettings.ShuffleOn 
                ? (ITrackSorter)new RandomTrackSorter() 
                : new DefaultTrackSorter();

            return tracks.SortUsing(sorter).ToList();
        }

        public void Reorder()
        {
            var sortedTracks = Sort(Tracks);

            _queue.Set(sortedTracks, CurrentTrack == null ? 0 : sortedTracks.IndexOf(CurrentTrack) + 1, false);

            ListUpdated?.Invoke();
        }
    }
}
