using System;
using System.Collections.Generic;
using Whip.Common.Model;

namespace Whip.Services.Interfaces.Singletons
{
    public interface IPlaylist
    {
        event Action ListUpdated;
        event Action<Track> CurrentTrackChanged;

        Track CurrentTrack { get; }

        string PlaylistName { get; }

        List<Track> Tracks { get; }

        void Set(string playlistName, List<Track> tracks, Track startAt);

        void Reorder();

        void MoveNext();

        void MovePrevious();
    }
}
