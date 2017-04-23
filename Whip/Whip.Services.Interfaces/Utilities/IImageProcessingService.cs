
namespace Whip.Services.Interfaces
{
    public interface IImageProcessingService
    {
        byte[] GetImageBytesFromFile(string fileLocation);

        byte[] GetImageBytesFromUrl(string url);
    }
}
