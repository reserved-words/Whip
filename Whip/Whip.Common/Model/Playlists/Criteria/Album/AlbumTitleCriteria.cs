
namespace Whip.Common.Model.Playlists.Criteria
{
    public class AlbumTitleCriteria : StringCriteria<Album>
    {
        public AlbumTitleCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.AlbumTitle, criteriaType, valueString, a => a.Title)
        {
        }
    }
}
