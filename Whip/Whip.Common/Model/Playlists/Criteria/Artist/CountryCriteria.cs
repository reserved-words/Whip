
namespace Whip.Common.Model.Playlists.Criteria
{
    public class CountryCriteria : StringCriteria<Artist>
    {
        public CountryCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.Country, criteriaType, valueString, a => a.City.Country)
        {
        }
    }
}
