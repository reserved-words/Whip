using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;

namespace Whip.Common.Singletons
{
    public class Playlist
    {
        public event Action ListUpdated;
        public event Action<Track> CurrentTrackChanged;

        private readonly Stack<Track> _toPlay = new Stack<Track>();
        private readonly Stack<Track> _played = new Stack<Track>();

        private Track _currentTrack;

        private Track CurrentTrack
        {
            get { return _currentTrack; }
            set
            {
                _currentTrack = value;
                CurrentTrackChanged?.Invoke(_currentTrack);
            }
        }

        public bool Any()
        {
            return _toPlay.Any() || _played.Any();
        }

        public void Set(List<Track> tracks, Track startAt)
        {
            var index = tracks.IndexOf(startAt);
            Set(tracks, index > 0 ? index : 0);
        }

        public void Set(List<Track> tracks, int startAt = 0)
        {
            _played.Clear();
            _toPlay.Clear();

            for (var i = 0; i < startAt; i++)
            {
                _played.Push(tracks[i]);
            }

            for (var i = tracks.Count - 1; i >= startAt; i--)
            {
                _toPlay.Push(tracks[i]);
            }

            ListUpdated?.Invoke();
        }

        public void MoveNext()
        {
            if (!_toPlay.Any())
            {
                CurrentTrack = null;
                return;
            }
            _played.Push(_toPlay.Pop());
            CurrentTrack = _played.Peek();
        }

        public void MovePrevious()
        {
            if (!_played.Any())
            {
                CurrentTrack = null;
                return;
            }

            if (_played.Count > 1)
            {
                _toPlay.Push(_played.Pop());
            }
            
            CurrentTrack = _played.Peek();
        }
    }
}
