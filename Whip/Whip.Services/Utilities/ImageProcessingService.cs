using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class ImageProcessingService : IImageProcessingService
    {
        public byte[] GetImageBytesFromFile(string fileLocation)
        {
            if (string.IsNullOrEmpty(fileLocation))
                return null;

            var img = Image.FromFile(fileLocation);

            if (img == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public byte[] GetImageBytesFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            using (var webClient = new WebClient())
            {
                return webClient.DownloadData(url);
            }
        }
    }
}
