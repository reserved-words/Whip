using System.Collections.Generic;
using Whip.Common.Model;
using Whip.Common.Singletons;

namespace Whip.Services.Interfaces
{
    public interface IDataPersistenceService
    {
        void Save(Library library);

        Library GetLibrary();
    }
}