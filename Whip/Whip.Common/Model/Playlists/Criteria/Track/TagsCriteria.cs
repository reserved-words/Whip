
namespace Whip.Common.Model.Playlists.Criteria
{
    public class TagsCriteria : StringListCriteria<Track>
    {
        public TagsCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.Tags, criteriaType, valueString, t => t.Tags)
        {
        }
    }
}
