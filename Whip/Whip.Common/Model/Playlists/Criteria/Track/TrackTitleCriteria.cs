
namespace Whip.Common.Model.Playlists.Criteria
{
    public class TrackTitleCriteria : StringCriteria<Track>
    {
        public TrackTitleCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.TrackTitle, criteriaType, valueString, t => t.Title)
        {
        }
    }
}
