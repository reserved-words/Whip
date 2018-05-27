using System;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface ILibrarySettings
    {
        string DataDirectory { get; }
        string MusicDirectory { get; set; }
        string ArchiveDirectory { get; set; }
    }
}
