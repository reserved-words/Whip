using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Model;

namespace Whip.Services.Interfaces.Singletons
{
    public interface IPlayRequestHandler
    {
        void MoveToTrack(Track track);
        void PlayAll(SortType? sortType, Track firstTrack = null);
        void PlayGrouping(string grouping, SortType? sortType, Track firstTrack = null);
        void PlayAlbum(Album album, SortType? sortType, Track firstTrack = null);
        void PlayArtist(Artist artist, SortType? sortType, Track firstTrack = null);
        void PlayPlaylist(string title, List<Track> tracks, SortType? sortType, Track firstTrack = null);
    }
}
