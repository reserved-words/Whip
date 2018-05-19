using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class PlaylistsService : IPlaylistsService
    {
        private readonly IPlaylistRepository _repository;

        public PlaylistsService(IPlaylistRepository repository)
        {
            _repository = repository;
        }

        public AllPlaylists GetPlaylists()
        {
            return _repository.GetPlaylists();
        }

        public List<Playlist> GetFavouritePlaylists()
        {
            return _repository.GetFavouritePlaylists();
        }

        public void Save(OrderedPlaylist playlist)
        {
            _repository.Save(playlist);
        }

        public void Save(CriteriaPlaylist playlist)
        {
            _repository.Save(playlist);
        }

        public void Save(QuickPlaylist playlist)
        {
            _repository.Save(playlist);
        }

        public void Delete(CriteriaPlaylist playlist)
        {
            _repository.Delete(playlist);
        }

        public void Delete(OrderedPlaylist playlist)
        {
            _repository.Delete(playlist);
        }

        public bool ValidatePlaylistTitle(string title, int id)
        {
            var allPlaylists = _repository.GetPlaylists();

            var playlists = new List<Playlist>();
            playlists.AddRange(allPlaylists.CriteriaPlaylists);
            playlists.AddRange(allPlaylists.OrderedPlaylists);
            playlists.AddRange(allPlaylists.FavouriteQuickPlaylists);

            return playlists.TrueForAll(pl => pl.Title != title || pl.Id == id);
        }

        public CriteriaPlaylist GetCriteriaPlaylist(int id)
        {
            return _repository.GetCriteriaPlaylist(id);
        }

        public OrderedPlaylist GetOrderedPlaylist(int id)
        {
            return _repository.GetOrderedPlaylist(id);
        }

        public QuickPlaylist GetQuickPlaylist(int id)
        {
            return _repository.GetQuickPlaylist(id);
        }

        public void RemoveTracks(List<Track> tracks)
        {
            var files = tracks.Select(t => t.File.FullPath).ToList();
            var playlists = _repository.GetOrderedPlaylists();

            foreach (var playlist in playlists)
            {
                var intersection = playlist.Tracks.Intersect(files).ToList();

                if (intersection.Any())
                {
                    intersection.ForEach(f => playlist.Tracks.Remove(f));
                    _repository.Save(playlist);
                }
            }
        }
    }
}
