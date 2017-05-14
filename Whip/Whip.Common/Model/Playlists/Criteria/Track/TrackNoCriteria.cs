
namespace Whip.Common.Model.Playlists.Criteria
{
    public class TrackNoCriteria : IntegerCriteria<Track>
    {
        public TrackNoCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.TrackNo, criteriaType, valueString, t => t.TrackNo)
        {
        }
    }
}
