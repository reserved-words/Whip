using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ICloudService
    {
        string GetArtworkUrl(Album album);
        string GetFileUrl(string filename);
        string GetTrackUrl(Track track);
        void UploadArtwork(Album album);
        void UploadFile(string path);
        void UploadTrack(Track track);
    }
}
