using System.Collections.Generic;
using Whip.Common.Model;
using Whip.Common.Singletons;

namespace Whip.Web.Interfaces
{
    public interface ILibraryService
    {
        void Reset();
        Library Library { get; }
        List<Track> Tracks { get; }
    }
}