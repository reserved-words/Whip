using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model.Playlists.Criteria
{
    public class InstrumentalCriteria : BooleanCriteria<Track>
    {
        public InstrumentalCriteria(CriteriaType criteriaType) 
            : base(PropertyName.Instrumental, criteriaType, t => t.Instrumental)
        {
        }
    }
}
