using System;
using System.Collections.Generic;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IPlaylistsService
    {
        AllPlaylists GetPlaylists();
        List<Playlist> GetFavouritePlaylists();
        void Save(OrderedPlaylist playlist);
        void Save(CriteriaPlaylist playlist);
        void Save(QuickPlaylist playlist);
        void Delete(CriteriaPlaylist playlist);
        void Delete(OrderedPlaylist playlist);
        bool ValidatePlaylistTitle(string title, int id);
        CriteriaPlaylist GetCriteriaPlaylist(int id);
        OrderedPlaylist GetOrderedPlaylist(int id);
        QuickPlaylist GetQuickPlaylist(int id);
        void RemoveTracks(List<Track> tracks);
    }
}
