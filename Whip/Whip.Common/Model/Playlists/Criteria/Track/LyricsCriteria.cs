
namespace Whip.Common.Model.Playlists.Criteria
{
    public class LyricsCriteria : StringCriteria<Track>
    {
        public LyricsCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.Lyrics, criteriaType, valueString, t => t.Lyrics)
        {
        }
    }
}
