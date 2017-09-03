using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IPlayRequestHandler
    {
        void MoveToTrack(Track track);
        void PlayAll(SortType? sortType, Track firstTrack = null);
        void PlayGrouping(string grouping, SortType? sortType, Track firstTrack = null);
        void PlayAlbum(Album album, SortType? sortType, Track firstTrack = null);
        void PlayArtist(Artist artist, SortType? sortType, Track firstTrack = null);
        void PlayPlaylist(string playlistName, List<Track> tracks, SortType? sortType, Track firstTrack = null);
    }
}
