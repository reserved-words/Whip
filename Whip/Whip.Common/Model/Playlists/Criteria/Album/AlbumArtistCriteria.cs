
namespace Whip.Common.Model.Playlists.Criteria
{
    public class AlbumArtistCriteria : StringCriteria<Album>
    {
        public AlbumArtistCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.AlbumArtist, criteriaType, valueString, a => a.Artist.Name)
        {
        }
    }
}
