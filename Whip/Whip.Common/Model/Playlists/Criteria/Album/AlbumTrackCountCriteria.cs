using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model.Playlists.Criteria
{
    public class AlbumTrackCountCriteria : IntegerCriteria<Album>
    {
        public AlbumTrackCountCriteria(string valueString, CriteriaType criteriaType)
            : base(PropertyName.AlbumTrackCount, criteriaType, valueString, a => a.Discs.Sum(d => d.Tracks.Count))
        {
        }
    }
}
