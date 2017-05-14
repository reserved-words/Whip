
namespace Whip.Common.Model.Playlists.Criteria
{
    public class GenreCriteria : StringCriteria<Artist>
    {
        public GenreCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.Genre, criteriaType, valueString, a => a.Genre)
        {
        }
    }
}
