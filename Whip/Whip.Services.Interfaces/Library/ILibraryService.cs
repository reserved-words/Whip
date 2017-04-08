using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Common.Utilities;

namespace Whip.Services.Interfaces
{
    public interface ILibraryService
    {
        Task<Library> GetLibraryAsync(IProgress<ProgressArgs> progressHandler);
        void SaveLibrary(Library library);
    }
}
