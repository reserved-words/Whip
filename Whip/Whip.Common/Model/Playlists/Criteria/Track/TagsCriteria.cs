
namespace Whip.Common.Model.Playlists.Criteria
{
    public class TagsCriteria : StringListCriteria<Track>
    {
        public TagsCriteria(string valueString)
            : base(PropertyName.Tags, CriteriaType.Contains, valueString, t => t.Tags)
        {
        }
    }
}
