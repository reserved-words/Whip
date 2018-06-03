using System;
using System.Collections.Generic;
using Whip.Common.Model;
using Whip.Common.Singletons;

namespace Whip.Web.Interfaces
{
    public interface IPlaylistService
    {
        AllPlaylists GetAll();
        Tuple<Playlist, List<Track>> GetCriteriaPlaylist(int id);
        Tuple<Playlist, List<Track>> GetOrderedPlaylist(int id);
        Tuple<Playlist, List<Track>> GetQuickPlaylist(int id);
        List<Playlist> GetFavourites();
    }
}