
namespace Whip.Common.Model.Playlists.Criteria
{
    public class GroupingCriteria : StringCriteria<Artist>
    {
        public GroupingCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.Grouping, criteriaType, valueString, a => a.Grouping)
        {
        }
    }
}
