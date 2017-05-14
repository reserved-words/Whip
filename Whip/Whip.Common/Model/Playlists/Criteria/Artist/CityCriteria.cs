
namespace Whip.Common.Model.Playlists.Criteria
{
    public class CityCriteria : StringCriteria<Artist>
    {
        public CityCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.City, criteriaType, valueString, a => a.City.Name)
        {
        }
    }
}
