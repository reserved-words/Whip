
namespace Whip.Common.Model.Playlists.Criteria
{
    public class ArtistCriteria : StringCriteria<Artist>
    {
        public ArtistCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.Artist, criteriaType, valueString, a => a.Name)
        {
        }
    }
}
