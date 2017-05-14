
namespace Whip.Common.Model.Playlists.Criteria
{
    public class DurationCriteria : TimeSpanCriteria<Track>
    {
        public DurationCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.Duration, criteriaType, valueString, t => t.Duration)
        {
        }
    }
}
