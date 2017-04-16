using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Common.Interfaces
{
    public interface IShowTabRequestHandler
    {
        event Action<Track> ShowEditTrackTab;
        event Action ShowSettingsTab;
    }
}
