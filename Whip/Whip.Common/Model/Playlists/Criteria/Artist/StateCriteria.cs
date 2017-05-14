
namespace Whip.Common.Model.Playlists.Criteria
{
    public class StateCriteria : StringCriteria<Artist>
    {
        public StateCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.State, criteriaType, valueString, a => a.City.State)
        {
        }
    }
}
