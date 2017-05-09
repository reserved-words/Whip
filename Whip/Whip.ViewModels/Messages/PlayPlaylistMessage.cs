using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Model;

namespace Whip.ViewModels.Messages
{
    public class PlayPlaylistMessage : PlayMessage
    {
        public PlayPlaylistMessage(string playlistName, SortType? sortType, List<Track> tracks, Track startAt = null)
            : base(sortType, startAt)
        {
            PlaylistName = playlistName;
            Tracks = tracks;
        }

        public string PlaylistName { get; private set; }
        public List<Track> Tracks { get; private set; }
    }
}
