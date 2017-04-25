using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
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

        public async Task<byte[]> GetImageBytesFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            using (var webClient = new WebClient())
            {
                return await webClient.DownloadDataTaskAsync(new Uri(url));
            }
        }

        public BitmapImage GetImageFromBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            var image = new BitmapImage();
            using (var mem = new MemoryStream(bytes))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        public async Task<BitmapImage> GetImageFromUrl(string url)
        {
            var bytes = await GetImageBytesFromUrl(url);
            return GetImageFromBytes(bytes);
        }
    }
}
