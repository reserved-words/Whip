using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;
using Whip.Common.Model.Playlists.Criteria;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly Library _library;
        private readonly IPlaylistCriteriaService _playlistCriteriaService;

        public PlaylistService(Library library, IPlaylistCriteriaService playlistCriteriaService)
        {
            _library = library;
            _playlistCriteriaService = playlistCriteriaService;
        } 

        public List<Track> GetTracks(CriteriaPlaylist playlist)
        {
            var tracks = new HashSet<Track>();

            foreach (var criteriaGroup in playlist.CriteriaGroups)
            {
                GetValidTracks(criteriaGroup).ForEach(t => tracks.Add(t));
            }

            IEnumerable<Track> list = tracks;

            if (playlist.OrderByProperty.HasValue)
            {
                var orderByFunction = _playlistCriteriaService.GetTrackPropertyFunction(playlist.OrderByProperty.Value);

                list = playlist.OrderByDescending 
                    ? list.OrderByDescending(t => orderByFunction(t))
                    : list.OrderBy(t => orderByFunction(t));
            }

            if (playlist.MaxTracks.HasValue)
            {
                list = list.Take(playlist.MaxTracks.Value);
            }

            return list.ToList();
        }

        private List<Track> GetValidTracks(CriteriaGroup criteriaGroup)
        {
            if (!criteriaGroup.DiscCriteria.Any() && !criteriaGroup.AlbumCriteria.Any())
            {
                return _library.Artists
                    .Where(a => criteriaGroup.ArtistCriteria.All(cr => cr.Predicate(a)))
                    .SelectMany(a => a.Tracks)
                    .Where(t => criteriaGroup.TrackCriteria.All(cr => cr.Predicate(t)))
                    .ToList();
            }

            if (!criteriaGroup.ArtistCriteria.Any())
            {
                return _library.Artists
                    .SelectMany(a => a.Albums)
                    .Where(a => criteriaGroup.AlbumCriteria.All(cr => cr.Predicate(a)))
                    .SelectMany(a => a.Discs)
                    .Where(d => criteriaGroup.DiscCriteria.All(cr => cr.Predicate(d)))
                    .SelectMany(a => a.Tracks)
                    .Where(t => criteriaGroup.TrackCriteria.All(cr => cr.Predicate(t)))
                    .ToList();
            }

            return _library.Artists
                .SelectMany(a => a.Albums)
                .Where(a => criteriaGroup.AlbumCriteria.All(cr => cr.Predicate(a)))
                .SelectMany(a => a.Discs)
                .Where(d => criteriaGroup.DiscCriteria.All(cr => cr.Predicate(d)))
                .SelectMany(a => a.Tracks)
                .Where(t => criteriaGroup.TrackCriteria.All(cr => cr.Predicate(t)) && criteriaGroup.ArtistCriteria.All(cr => cr.Predicate(t.Artist)))
                .ToList();
        }

        
    }
}
