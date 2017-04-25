
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Whip.Services.Interfaces
{
    public interface IImageProcessingService
    {
        byte[] GetImageBytesFromFile(string fileLocation);

        Task<byte[]> GetImageBytesFromUrl(string url);

        BitmapImage GetImageFromBytes(byte[] bytes);
        Task<BitmapImage> GetImageFromUrl(string url);
    }
}
