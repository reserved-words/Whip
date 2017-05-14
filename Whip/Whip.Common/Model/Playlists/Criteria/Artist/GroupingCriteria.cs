
namespace Whip.Common.Model.Playlists.Criteria
{
    public class GroupingCriteria : StringCriteria<Artist>
    {
        public GroupingCriteria(string valueString)
            : base(PropertyName.Grouping, CriteriaType.IsEqualTo, valueString, a => a.Grouping)
        {
        }
    }
}
