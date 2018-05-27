using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ICloudService
    {
        string GetFileUrl(string filename);
        string GetTrackUrl(Track track);
        void UploadFile(string path);
        void UploadTrack(Track track);
    }
}
