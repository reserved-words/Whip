using System.Collections.Generic;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IPlaylistRepository
    {
        AllPlaylists GetPlaylists();
        List<Playlist> GetFavouritePlaylists();
        void Save(OrderedPlaylist playlist);
        void Save(CriteriaPlaylist playlist);
        void Save(QuickPlaylist playlist);
        void Delete(CriteriaPlaylist playlist);
        void Delete(OrderedPlaylist playlist);
        CriteriaPlaylist GetCriteriaPlaylist(int id);
        OrderedPlaylist GetOrderedPlaylist(int id);
        QuickPlaylist GetQuickPlaylist(int id);
        List<OrderedPlaylist> GetOrderedPlaylists();
    }
}
