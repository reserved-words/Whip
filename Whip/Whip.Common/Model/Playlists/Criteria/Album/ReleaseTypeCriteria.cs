
namespace Whip.Common.Model.Playlists.Criteria
{
    public class ReleaseTypeCriteria : EnumCriteria<Album, ReleaseType>
    {
        public ReleaseTypeCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.ReleaseType, criteriaType, valueString, a => a.ReleaseType)
        {
        }
    }
}
