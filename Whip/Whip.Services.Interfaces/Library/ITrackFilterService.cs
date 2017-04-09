using System;
using System.Collections.Generic;
using Whip.Common.Enums;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Common.Utilities;

namespace Whip.Services.Interfaces
{
    public interface ITrackFilterService
    {
        List<Track> GetAll(ITrackSorter sorter);
    }
}
