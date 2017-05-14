using System;
using System.Linq;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Model.Playlists.Criteria;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    // TO DO: Is there a way to do this without switch statements?
    public class PlaylistCriteriaService : IPlaylistCriteriaService
    {
        public Criteria<Album> GetAlbumCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString)
        {
            switch (propertyName)
            {
                case PropertyName.DiscCount:
                    return new DiscCountCriteria(valueString, criteriaType);
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
                default:
                    throw new InvalidOperationException();
            }
        }

        public Func<Track, object> GetTrackPropertyFunction(PropertyName propertyName)
        {
            switch (propertyName)
            {
                case PropertyName.TrackTitle:
                    return t => t.Title;
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
                case PropertyName.AlbumTitle:
                    return t => t.Disc.Album.Title;
                case PropertyName.AlbumTrackCount:
                    return t => t.Disc.Album.Discs.Sum(d => d.Tracks.Count);
                case PropertyName.ReleaseType:
                    return t => t.Disc.Album.ReleaseType;
                case PropertyName.ReleaseYear:
                    return t => t.Disc.Album.Year;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
