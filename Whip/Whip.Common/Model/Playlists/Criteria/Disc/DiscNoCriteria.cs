
namespace Whip.Common.Model.Playlists.Criteria
{
    public class DiscNoCriteria : IntegerCriteria<Disc>
    {
        public DiscNoCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.DiscNo, criteriaType, valueString, d => d.DiscNo)
        {
        }
    }
}
