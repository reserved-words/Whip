using System.Collections.Generic;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IPlaylistService
    {
        List<Track> GetTracks(CriteriaPlaylist playlist);
    }
}
