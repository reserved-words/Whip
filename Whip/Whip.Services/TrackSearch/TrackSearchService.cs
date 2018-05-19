using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common;
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

        public TrackSearchService(Library library)
        {
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
                var orderByFunction = GetTrackOrderByFunction(trackCriteria.OrderByProperty.Value);

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

        public Criteria<Album> GetAlbumCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString)
        {
            switch (propertyName)
            {
                case PropertyName.DiscCount:
                    return new DiscCountCriteria(valueString, criteriaType);
                case PropertyName.AlbumArtist:
                    return new AlbumArtistCriteria(valueString, criteriaType);
                case PropertyName.AlbumTitle:
                    return new AlbumTitleCriteria(valueString, criteriaType);
                case PropertyName.AlbumTrackCount:
                    return new AlbumTrackCountCriteria(valueString, criteriaType);
                case PropertyName.ReleaseType:
                    return new ReleaseTypeCriteria(valueString, criteriaType);
                case PropertyName.ReleaseYear:
                    return new ReleaseYearCriteria(valueString, criteriaType);
                default:
                    throw new InvalidOperationException();
            }
        }

        public Criteria<Artist> GetArtistCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString)
        {
            switch (propertyName)
            {
                case PropertyName.Artist:
                    return new ArtistCriteria(valueString, criteriaType);
                case PropertyName.City:
                    return new CityCriteria(valueString, criteriaType);
                case PropertyName.State:
                    return new StateCriteria(valueString, criteriaType);
                case PropertyName.Country:
                    return new CountryCriteria(valueString, criteriaType);
                case PropertyName.Grouping:
                    return new GroupingCriteria(valueString, criteriaType);
                case PropertyName.Genre:
                    return new GenreCriteria(valueString, criteriaType);
                default:
                    throw new InvalidOperationException();
            }
        }

        public Criteria<Disc> GetDiscCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString)
        {
            switch (propertyName)
            {
                case PropertyName.DiscNo:
                    return new DiscNoCriteria(valueString, criteriaType);
                case PropertyName.DiscTrackCount:
                    return new DiscTrackCountCriteria(valueString, criteriaType);
                default:
                    throw new InvalidOperationException();
            }
        }

        public Criteria<Track> GetTrackCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString)
        {
            switch (propertyName)
            {
                case PropertyName.Lyrics:
                    return new LyricsCriteria(valueString, criteriaType);
                case PropertyName.TrackTitle:
                    return new TrackTitleCriteria(valueString, criteriaType);
                case PropertyName.Tags:
                    return new TagsCriteria(valueString, criteriaType);
                case PropertyName.Duration:
                    return new DurationCriteria(valueString, criteriaType);
                case PropertyName.DateAdded:
                    return new DateAddedCriteria(valueString, criteriaType);
                case PropertyName.DateModified:
                    return new DateModifiedCriteria(valueString, criteriaType);
                case PropertyName.TrackNo:
                    return new TrackNoCriteria(valueString, criteriaType);
                case PropertyName.TrackYear:
                    return new TrackYearCriteria(valueString, criteriaType);
                case PropertyName.Instrumental:
                    return new InstrumentalCriteria(criteriaType);
                default:
                    throw new InvalidOperationException();
            }
        }

        public Func<Track, object> GetTrackOrderByFunction(PropertyName propertyName)
        {
            switch (propertyName)
            {
                case PropertyName.TrackTitle:
                    return t => t.Title;
                case PropertyName.Lyrics:
                    return t => t.Lyrics;
                case PropertyName.Tags:
                    return t => t.Tags;
                case PropertyName.Duration:
                    return t => t.Duration;
                case PropertyName.DateAdded:
                    return t => t.File.DateCreated;
                case PropertyName.DateModified:
                    return t => t.File.DateModified;
                case PropertyName.TrackNo:
                    return t => t.TrackNo;
                case PropertyName.TrackYear:
                    return t => t.Year;
                case PropertyName.DiscNo:
                    return t => t.Disc.DiscNo;
                case PropertyName.DiscTrackCount:
                    return t => t.Disc.Tracks.Count;
                case PropertyName.Artist:
                    return t => t.Artist.Sort;
                case PropertyName.City:
                    return t => t.Artist.City.Name;
                case PropertyName.State:
                    return t => t.Artist.City.State;
                case PropertyName.Country:
                    return t => t.Artist.City.Country;
                case PropertyName.Grouping:
                    return t => t.Artist.Grouping;
                case PropertyName.Genre:
                    return t => t.Artist.Genre;
                case PropertyName.DiscCount:
                    return t => t.Disc.Album.Discs.Count;
                case PropertyName.AlbumArtist:
                    return t => t.Disc.Album.Artist.Sort;
                case PropertyName.AlbumTitle:
                    return t => t.Disc.Album.Title;
                case PropertyName.AlbumTrackCount:
                    return t => t.Disc.Album.Discs.Sum(d => d.Tracks.Count);
                case PropertyName.ReleaseType:
                    return t => t.Disc.Album.ReleaseType;
                case PropertyName.ReleaseYear:
                    return t => t.Disc.Album.Year;
                case PropertyName.Instrumental:
                    return t => t.Instrumental ? 0 : 1;
                default:
                    throw new InvalidOperationException();
            }
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
