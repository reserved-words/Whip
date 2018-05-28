using System;
using System.Collections.Generic;
using Whip.Common.Enums;
using Whip.Common.Interfaces;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ITrackSearchService
    {
        List<Track> GetTracks(ITrackCriteria trackCriteria);
        List<Track> GetTracks(List<string> filepaths);
        List<Track> GetTracks(FilterType filterType, params string[] filterValues);
    }
}
