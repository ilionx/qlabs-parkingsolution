using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using ProjectParking.Contracts;

namespace ProjectParking.Forwarders.BlobStorageForwarder.Resources
{
    public class MessagesBlobStorageRepository : IMessageRepository
    {
        private readonly ILogger _logger;
        private readonly CloudBlobContainer _cloudBlobContainer;

        public MessagesBlobStorageRepository(ILogger logger)
        {
            _logger = logger;

            //todo refactor to proper DI
            var storageAccount =
                CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"]
                    .ConnectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();

            _cloudBlobContainer = blobClient.GetContainerReference("parkingspotupdates");
            _cloudBlobContainer.CreateIfNotExists();
        }

        public async Task<string> Store(IParkingSpotStatusUpdate update)
        {
            var fileName = $"{update.Timestamp.Ticks}-{Guid.NewGuid()}.json";

            _logger.LogInformation($"Uploading {fileName}");

            try
            {
                var json = JsonConvert.SerializeObject(update);
                CloudBlockBlob cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(fileName);

                await cloudBlockBlob.UploadTextAsync(json);

                _logger.LogInformation($"Uploaded {fileName} @ {cloudBlockBlob.Uri}");

                return cloudBlockBlob.Uri.ToString();
            }
            catch (Exception exception)
            {
                _logger.LogError($"Uploading {fileName} failed.", exception);
                return string.Empty;
            }
        }
    }
}
