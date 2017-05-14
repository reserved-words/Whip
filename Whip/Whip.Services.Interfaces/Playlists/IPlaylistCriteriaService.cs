using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Model.Playlists.Criteria;

namespace Whip.Services.Interfaces
{
    public interface IPlaylistCriteriaService
    {
        Criteria<Artist> GetArtistCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString);

        Criteria<Track> GetTrackCriteria(PropertyName propertyName, CriteriaType criteriaType, string valueString);
    }
}
