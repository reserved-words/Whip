using System.Collections.Generic;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IPlaylistRepository
    {
        AllPlaylists GetPlaylists();

        void Save(OrderedPlaylist playlist);

        void Save(CriteriaPlaylist playlist);

        void Delete(CriteriaPlaylist playlist);

        bool ValidatePlaylistTitle(string title, int id);
    }
}
