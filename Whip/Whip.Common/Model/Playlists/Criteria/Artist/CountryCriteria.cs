
namespace Whip.Common.Model.Playlists.Criteria
{
    public class CountryCriteria : StringCriteria<Artist>
    {
        public CountryCriteria(string valueString)
            : base(PropertyName.Country, CriteriaType.IsEqualTo, valueString, a => a.City.Country)
        {
        }
    }
}
