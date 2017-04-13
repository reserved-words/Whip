using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ITrackFilterService
    {
        List<Track> GetAll(SortType sortType);
        List<Track> GetTracksByGrouping(string grouping, SortType sortType);
        List<Track> GetTracksByArtists(List<Artist> artists, SortType sortType);
        List<Track> GetAlbumTracksByArtists(List<Artist> artists, SortType sortType);
        List<Track> GetTracksFromAlbums(List<Album> albums, SortType sortType);
    }
}
