
namespace Whip.Common.Model.Playlists.Criteria
{
    public class DiscCountCriteria : IntegerCriteria<Album>
    {
        public DiscCountCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.DiscCount, criteriaType, valueString, a => a.Discs.Count)
        {
        }
    }
}
