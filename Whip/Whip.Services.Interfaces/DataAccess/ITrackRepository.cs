using Whip.Common.Singletons;

namespace Whip.Services.Interfaces
{
    public interface ITrackRepository
    {
        void Save(Library library);

        Library GetLibrary();
    }
}