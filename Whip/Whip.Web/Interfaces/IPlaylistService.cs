using System;
using System.Collections.Generic;
using Whip.Common.Model;
using Whip.Common.Singletons;

namespace Whip.Web.Interfaces
{
    public interface IPlaylistService
    {
        AllPlaylists GetAll();
        Tuple<Playlist, List<Track>> GetCriteriaPlaylist(int id, Library library);
        Tuple<Playlist, List<Track>> GetOrderedPlaylist(int id, Library library);
        Tuple<Playlist, List<Track>> GetQuickPlaylist(int id, Library library);
        List<Playlist> GetFavourites();
    }
}