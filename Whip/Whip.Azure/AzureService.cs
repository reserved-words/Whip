using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.AzureSync
{
    public class AzureService : ICloudService
    {
        private const string UrlFormat = "https://{0}.blob.core.windows.net/{1}/{2}";

        private readonly Lazy<CloudBlobContainer> _container = new Lazy<CloudBlobContainer>(GetContainerReference);

        public void Upload(Track track)
        {
            var cloudBlockBlob = _container.Value.GetBlockBlobReference(GetBlobPath(track));
            var task = Task.Run(async () => await cloudBlockBlob.UploadFromFileAsync(track.File.FullPath));
            task.Wait();
        }

        public string GetUrl(Track track)
        {
            return string.Format(UrlFormat, GetAccountName(), GetContainerName(), GetBlobPath(track));
        }

        private static string GetBlobPath(Track track)
        {
            var parentFolder = track.Disc.Album.Artist.Name.ToLowerInvariant();
            var childFolder = track.Disc.Album.Title.ToLowerInvariant();
            var filename = Path.GetFileName(track.File.FullPath);
            return $"{parentFolder}/{childFolder}/{filename}";
        }

        private static string GetAccountName()
        {
            throw new NotImplementedException();
        }

        private static string GetContainerName()
        {
            throw new NotImplementedException();
        }

        private static string GetConnectionString()
        {
            throw new NotImplementedException();
        }

        private static CloudBlobContainer GetContainerReference()
        {
            var connectionString = GetConnectionString();
            var containerName = GetContainerName();

            CloudStorageAccount storageAccount;

            if (CloudStorageAccount.TryParse(connectionString, out storageAccount))
            {
                var client = storageAccount.CreateCloudBlobClient();
                return client.GetContainerReference(containerName);
            }
            else
            {
                return null;
            }
        }
    }
}
