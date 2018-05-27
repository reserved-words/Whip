using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ICloudService
    {
        void Upload(Track track);
        string GetUrl(Track track);
    }
}
