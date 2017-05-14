
namespace Whip.Common.Model.Playlists.Criteria
{
    public class DiscTrackCountCriteria : IntegerCriteria<Disc>
    {
        public DiscTrackCountCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.DiscTrackCount, criteriaType, valueString, d => d.Tracks.Count)
        {
        }
    }
}
