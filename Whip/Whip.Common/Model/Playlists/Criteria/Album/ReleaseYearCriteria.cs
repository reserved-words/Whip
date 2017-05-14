
namespace Whip.Common.Model.Playlists.Criteria
{
    public class ReleaseYearCriteria : StringCriteria<Album>
    {
        public ReleaseYearCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.ReleaseYear, criteriaType, valueString, a => a.Year)
        {
        }
    }
}
