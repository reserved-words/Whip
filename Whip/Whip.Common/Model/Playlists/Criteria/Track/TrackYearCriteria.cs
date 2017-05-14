
namespace Whip.Common.Model.Playlists.Criteria
{
    public class TrackYearCriteria : StringCriteria<Track>
    {
        public TrackYearCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.TrackYear, criteriaType, valueString, t => t.Year)
        {
        }
    }
}
