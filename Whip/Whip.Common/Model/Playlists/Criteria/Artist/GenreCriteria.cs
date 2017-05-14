
namespace Whip.Common.Model.Playlists.Criteria
{
    public class GenreCriteria : StringCriteria<Artist>
    {
        public GenreCriteria(string valueString)
            : base(PropertyName.Genre, CriteriaType.IsEqualTo, valueString, a => a.Genre)
        {
        }
    }
}
