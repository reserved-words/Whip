using System.Collections.Generic;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IDataPersistenceService
    {
        void Save(ICollection<Artist> artists);

        Library GetLibrary();
    }
}