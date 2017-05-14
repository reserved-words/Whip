using System;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Model.Playlists.Criteria;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class PlaylistCriteriaService : IPlaylistCriteriaService
    {
        public Criteria<Artist> GetArtistCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString)
        {
            switch (propertyName)
            {
                case PropertyName.Country:
                    return new CountryCriteria(valueString);
                case PropertyName.Grouping:
                    return new GroupingCriteria(valueString);
                case PropertyName.Genre:
                    return new GenreCriteria(valueString);
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
                    return new TagsCriteria(valueString);
                case PropertyName.Duration:
                    return new DurationCriteria(valueString, criteriaType);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
