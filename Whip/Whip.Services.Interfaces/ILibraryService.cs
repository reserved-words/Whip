using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Common.Utilities;

namespace Whip.Services.Interfaces
{
    public interface ILibraryService
    {
        Task<ICollection<Artist>> GetLibraryAsync(string directory, string[] extensions, IProgress<ProgressArgs> progressHandler);
        void SaveLibrary(ICollection<Artist> artists);
    }
}
