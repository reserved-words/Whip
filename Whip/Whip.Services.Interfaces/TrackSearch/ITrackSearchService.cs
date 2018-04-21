using System;
using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Enums;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Common.Model.Playlists.Criteria;

namespace Whip.Services.Interfaces
{
    public interface ITrackSearchService
    {
        List<Track> GetTracks(ITrackCriteria trackCriteria);
        List<Track> GetTracks(List<string> filepaths);
        Criteria<Album> GetAlbumCriteria(PropertyName value1, CriteriaType value2, string valueString);
        Criteria<Artist> GetArtistCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString);
        Criteria<Disc> GetDiscCriteria(PropertyName value1, CriteriaType value2, string valueString);
        Criteria<Track> GetTrackCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString);
        Func<Track, object> GetTrackOrderByFunction(PropertyName propertyName);
        List<Track> GetTracks(FilterType filterType, params string[] filterValues);
    }
}
