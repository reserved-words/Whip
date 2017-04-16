using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ITrackFilterService
    {
        List<Track> GetAll();
        List<Track> GetTracksByGrouping(string grouping);
        List<Track> GetTracksByArtist(Artist artist);
        List<Track> GetAlbumTracksByArtist(Artist artist);
        List<Track> GetTracksFromAlbum(Album album);
    }
}
