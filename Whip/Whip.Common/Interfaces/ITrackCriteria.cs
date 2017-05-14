using System.Collections.Generic;
using Whip.Common.Model.Playlists.Criteria;

namespace Whip.Common.Interfaces
{
    public interface ITrackCriteria
    {
        List<CriteriaGroup> CriteriaGroups { get; }

        PropertyName? OrderByProperty { get; }

        bool OrderByDescending { get; }

        int? MaxTracks { get; }
    }
}
