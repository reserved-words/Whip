
namespace Whip.Common.Model.Playlists.Criteria
{
    public class DateModifiedCriteria : DateTimeCriteria<Track>
    {
        public DateModifiedCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.DateAdded, criteriaType, valueString, t => t.File.DateModified)
        {
        }
    }
}
