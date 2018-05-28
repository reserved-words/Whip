using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Azure
{
    public class AzureService : ICloudService
    {
        private const string UrlFormat = "https://{0}.blob.core.windows.net/{1}/{2}";

        private CloudBlobContainer _container;

        private readonly IAzureStorageConfig _config;

        public AzureService()
        {
            _config = new AzureConfig();
        }

        public void UploadFile(string path)
        {
            UploadFile(Path.GetFileName(path), path);
        }

        public void UploadTrack(Track track)
        {
            UploadFile(GetBlobPath(track), track.File.FullPath);
        }

        public void UploadArtwork(Album album)
        {
            UploadBytes(GetImageBlobPath(album), album.Artwork);
        }

        public string GetArtworkUrl(Album album)
        {
            return string.Format(UrlFormat, _config.AccountName, _config.ContainerName, GetImageBlobPath(album));
        }

        public string GetFileUrl(string filename)
        {
            return string.Format(UrlFormat, _config.AccountName, _config.ContainerName, filename);
        }

        public string GetTrackUrl(Track track)
        {
            return string.Format(UrlFormat, _config.AccountName, _config.ContainerName, GetBlobPath(track));
        }

        private void UploadBytes(string blobPath, byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return;

            var cloudBlockBlob = Container.GetBlockBlobReference(blobPath);
            var task = Task.Run(async () => await cloudBlockBlob.UploadFromByteArrayAsync(bytes, 0, bytes.Length));
            task.Wait();
        }

        private void UploadFile(string blobPath, string filePath)
        {
            var cloudBlockBlob = Container.GetBlockBlobReference(blobPath);
            var task = Task.Run(async () => await cloudBlockBlob.UploadFromFileAsync(filePath));
            task.Wait();
        }

        private static string GetBlobPath(Track track)
        {
            var filename = Path.GetFileName(track.File.FullPath);
            return $"{GetBlobFolder(track.Disc.Album)}/{filename}";
        }

        private static string GetImageBlobPath(Album album)
        {
            return $"{GetBlobFolder(album)}/artwork.jpg";
        }

        private static string GetBlobFolder(Album album)
        {
            var parentFolder = album.Artist.Name.ToLowerInvariant();
            var childFolder = album.Title.ToLowerInvariant();
            return $"{parentFolder}/{childFolder}";
        }

        private CloudBlobContainer Container
        {
            get
            {
                if (_container != null)
                    return _container;

                var connectionString = _config.ConnectionString;
                var containerName = _config.ContainerName;

                CloudStorageAccount storageAccount;

                if (CloudStorageAccount.TryParse(connectionString, out storageAccount))
                {
                    var client = storageAccount.CreateCloudBlobClient();
                    _container = client.GetContainerReference(containerName);
                }
                else
                {
                    return null;
                }

                return _container;
            }
        }
    }
}
