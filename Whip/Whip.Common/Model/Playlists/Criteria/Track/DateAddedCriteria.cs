
namespace Whip.Common.Model.Playlists.Criteria
{
    public class DateAddedCriteria : DateTimeCriteria<Track>
    {
        public DateAddedCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.DateAdded, criteriaType, valueString, t => t.File.DateCreated)
        {
        }
    }
}
