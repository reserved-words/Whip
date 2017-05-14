using System;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Model.Playlists.Criteria;

namespace Whip.Services.Interfaces
{
    public interface IPlaylistCriteriaService
    {
        Criteria<Album> GetAlbumCriteria(PropertyName value1, CriteriaType value2, string valueString);
        Criteria<Artist> GetArtistCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString);
        Criteria<Disc> GetDiscCriteria(PropertyName value1, CriteriaType value2, string valueString);
        Criteria<Track> GetTrackCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString);
        Func<Track, object> GetTrackPropertyFunction(PropertyName propertyName);
    }
}
