using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Enums;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Common.Model.Playlists.Criteria;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class TrackSearchService : ITrackSearchService
    {
        private readonly Library _library;
        private readonly ITrackCriteriaService _criteriaService;

        public TrackSearchService(Library library, ITrackCriteriaService criteriaService)
        {
            _criteriaService = criteriaService;
            _library = library;
        } 

        public List<Track> GetTracks(ITrackCriteria trackCriteria)
        {
            if (!trackCriteria.CriteriaGroups.Any()
                && !trackCriteria.OrderByProperty.HasValue
                && !trackCriteria.MaxTracks.HasValue)
            {
                return new List<Track>();
            }

            var tracks = new HashSet<Track>();

            if (!trackCriteria.CriteriaGroups.Any())
            {
                _library.Artists.SelectMany(a => a.Tracks)
                    .ToList()
                    .ForEach(t => tracks.Add(t));
            }
            else
            {
                foreach (var criteriaGroup in trackCriteria.CriteriaGroups)
                {
                    GetValidTracks(criteriaGroup).ForEach(t => tracks.Add(t));
                }
            }

            IEnumerable<Track> list = tracks;

            if (trackCriteria.OrderByProperty.HasValue)
            {
                var orderByFunction = _criteriaService.GetTrackOrderByFunction(trackCriteria.OrderByProperty.Value);

                list = trackCriteria.OrderByDescending 
                    ? list.OrderByDescending(t => orderByFunction(t))
                    : list.OrderBy(t => orderByFunction(t));
            }

            if (trackCriteria.MaxTracks.HasValue)
            {
                list = list.Take(trackCriteria.MaxTracks.Value);
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
        
        public List<Track> GetTracks(FilterType filterType, params string[] filterValues)
        {
            switch (filterType)
            {
                case FilterType.City:
                    return _library.Artists.Where(a => a.City.Name == filterValues[0]
                            && a.City.State == filterValues[1]
                            && a.City.Country == filterValues[2])
                        .SelectMany(a => a.Tracks)
                        .ToList();
                case FilterType.Country:
                    return _library.Artists.Where(a => a.City.Country == filterValues[0])
                        .SelectMany(a => a.Tracks)
                        .ToList();
                case FilterType.DateAdded:
                    var days = int.Parse(filterValues[0]);
                    var minimumDate = DateTime.Now.AddDays(-1 * days);
                    return _library.Artists
                        .SelectMany(a => a.Tracks)
                        .Where(t => t.File.DateCreated >= minimumDate)
                        .ToList();
                case FilterType.Genre:
                    return _library.Artists.Where(a => a.Genre == filterValues[0])
                        .SelectMany(a => a.Tracks)
                        .ToList();
                case FilterType.Grouping:
                    return _library.Artists.Where(a => a.Grouping == filterValues[0])
                        .SelectMany(a => a.Tracks)
                        .ToList();
                case FilterType.State:
                    return _library.Artists.Where(a => a.City.State == filterValues[0]
                                                       && a.City.Country == filterValues[1])
                        .SelectMany(a => a.Tracks)
                        .ToList();
                case FilterType.Tag:
                    return _library.Artists
                        .SelectMany(a => a.Tracks)
                        .Where(t => t.Tags.Contains(filterValues[0]))
                        .ToList();
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterType), filterType, null);
            }
        }

        public List<Track> GetTracks(List<string> filepaths)
        {
            var tracks = new List<Track>();

            foreach (var fp in filepaths)
            {
                var track = _library.Artists.SelectMany(a => a.Tracks).SingleOrDefault(t => t.File.FullPath == fp);

                if (track != null)
                {
                    tracks.Add(track);
                }
            }

            return tracks;
        }
    }
}
