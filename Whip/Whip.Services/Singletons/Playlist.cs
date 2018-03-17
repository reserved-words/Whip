using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.ExtensionMethods;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Services.Singletons
{
    public class Playlist : IPlaylist
    {
        private readonly ITrackQueue _trackQueue;
        private readonly IRandomTrackSorter _randomTrackSorter;
        private readonly IDefaultTrackSorter _defaultTrackSorter;

        private bool _doNotSort;

        public Playlist(ITrackQueue trackQueue, IDefaultTrackSorter defaultTrackSorter, IRandomTrackSorter randomTrackSorter)
        {
            _trackQueue = trackQueue;
            _randomTrackSorter = randomTrackSorter;
            _defaultTrackSorter = defaultTrackSorter;
        }

        public event Action ListUpdated;
        public event Action<Track> CurrentTrackChanged;

        public Track CurrentTrack => _trackQueue.CurrentTrack;

        public string PlaylistName { get; private set; }

        public List<Track> Tracks { get; private set; }
        
        public void Set(string playlistName, List<Track> tracks, Track startAt, bool shuffle, bool? doNotSort = null)
        {
            PlaylistName = playlistName;
            Tracks = tracks;
            
            if (doNotSort.HasValue)
            {
                _doNotSort = doNotSort.Value;
            }

            var sortedTracks = Sort(tracks, shuffle);

            _trackQueue.Set(sortedTracks, startAt == null ? 0 : sortedTracks.IndexOf(startAt), true);

            OnListUpdated();
        }

        public void MoveNext()
        {
            _trackQueue.MoveNext();
            CurrentTrackChanged?.Invoke(_trackQueue.CurrentTrack);
        }

        public void MovePrevious()
        {
            _trackQueue.MovePrevious();
            CurrentTrackChanged?.Invoke(_trackQueue.CurrentTrack);
        }

        private List<Track> Sort(List<Track> tracks, bool shuffle)
        {
            if (_doNotSort && !shuffle)
                return tracks;

            var sorter = shuffle
                ? (ITrackSorter)_randomTrackSorter
                : _defaultTrackSorter;

            return tracks.SortUsing(sorter).ToList();
        }

        public void Reorder(bool shuffle)
        {
            var sortedTracks = Sort(Tracks, shuffle);

            _trackQueue.Set(sortedTracks, CurrentTrack == null ? 0 : sortedTracks.IndexOf(CurrentTrack) + 1, false);

            OnListUpdated();
        }

        private void OnListUpdated()
        {
            if (CurrentTrack == null)
            {
                MoveNext();
            }

            ListUpdated?.Invoke();
        }
    }
}
