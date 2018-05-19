using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class TrackQueue : ITrackQueue
    {
        private readonly Stack<Track> _toPlay = new Stack<Track>();
        private readonly Stack<Track> _played = new Stack<Track>();

        private Track _currentTrack;

        public Track CurrentTrack
        {
            get { return _currentTrack; }
            private set
            {
                if (_currentTrack != null)
                    _currentTrack.IsCurrentTrack = false;

                _currentTrack = value;

                if (_currentTrack != null)
                    _currentTrack.IsCurrentTrack = true;
            }
        }

        public void Set(List<Track> tracks, int startAt, bool clearCurrentTrack)
        {
            _played.Clear();
            _toPlay.Clear();

            if (clearCurrentTrack)
            {
                CurrentTrack = null;
            }

            for (var i = 0; i < startAt; i++)
            {
                _played.Push(tracks[i]);
            }

            for (var i = tracks.Count - 1; i >= startAt; i--)
            {
                _toPlay.Push(tracks[i]);
            }
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

        public void Remove(List<Track> tracks)
        {
            var toPlay = _toPlay.Reverse().ToList();
            var played = _played.Reverse().ToList();

            foreach (var track in tracks)
            {
                toPlay.Remove(track);
                played.Remove(track);
            }
            
            _toPlay.Clear();
            _played.Clear();

            toPlay.ForEach(_toPlay.Push);
            played.ForEach(_played.Push);
        }
    }
}
