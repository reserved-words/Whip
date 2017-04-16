using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ITrackFilterService
    {
        List<Track> GetAll(SortType sortType);
        List<Track> GetTracksByGrouping(string grouping, SortType sortType);
        List<Track> GetTracksByArtist(Artist artist, SortType sortType);
        List<Track> GetAlbumTracksByArtist(Artist artist, SortType sortType);
        List<Track> GetTracksFromAlbum(Album album, SortType sortType);
    }
}
