using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;
using Whip.Azure;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.AzureSync
{
    public class AzureService : ICloudService
    {
        private const string UrlFormat = "https://{0}.blob.core.windows.net/{1}/{2}";

        private CloudBlobContainer _container;

        private readonly IAzureStorageConfig _config;

        public AzureService(IAzureStorageConfig config)
        {
            _config = config;
        }

        public void UploadFile(string path)
        {
            UploadFile(Path.GetFileName(path), path);
        }

        public void UploadTrack(Track track)
        {
            UploadFile(GetBlobPath(track), track.File.FullPath);
        }

        public string GetFileUrl(string filename)
        {
            return string.Format(UrlFormat, _config.AccountName, _config.ContainerName, filename);
        }

        public string GetTrackUrl(Track track)
        {
            return string.Format(UrlFormat, _config.AccountName, _config.ContainerName, GetBlobPath(track));
        }

        private void UploadFile(string blobPath, string filePath)
        {
            var cloudBlockBlob = Container.GetBlockBlobReference(blobPath);
            var task = Task.Run(async () => await cloudBlockBlob.UploadFromFileAsync(filePath));
            task.Wait();
        }

        private static string GetBlobPath(Track track)
        {
            var parentFolder = track.Disc.Album.Artist.Name.ToLowerInvariant();
            var childFolder = track.Disc.Album.Title.ToLowerInvariant();
            var filename = Path.GetFileName(track.File.FullPath);
            return $"{parentFolder}/{childFolder}/{filename}";
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
