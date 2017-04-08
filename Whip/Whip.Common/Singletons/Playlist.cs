using System;
using System.Collections.Generic;
using Whip.Common.Model;

namespace Whip.Common.Singletons
{
    public class Playlist
    {
        public event Action Updated;

        private readonly Queue<Track> _toPlay = new Queue<Track>();
        private readonly Stack<Track> _played = new Stack<Track>();

        public void Set(List<Track> tracks, int startAt = 0)
        {
            _played.Clear();
            _toPlay.Clear();

            for (var i = 0; i < startAt; i++)
            {
                _played.Push(tracks[i]);
            }

            for (var i = startAt; i < tracks.Count; i++)
            {
                _toPlay.Enqueue(tracks[i]);
            }

            Updated?.Invoke();
        }

        public Track GetNext()
        {
            _played.Push(_toPlay.Dequeue());
            return _played.Peek();
        }

        public Track GetPrevious()
        {
            _toPlay.Enqueue(_played.Pop());
            return _played.Peek();
        }
    }
}
