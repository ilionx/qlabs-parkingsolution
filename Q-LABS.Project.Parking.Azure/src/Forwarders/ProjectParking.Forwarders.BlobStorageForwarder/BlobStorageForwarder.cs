using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ProjectParking.Contracts;
using ProjectParking.Forwarders.BlobStorageForwarder.Resources;

namespace ProjectParking.Forwarders.BlobStorageForwarder
{
    public class BlobStorageForwarder
    {
        private readonly IMessageRepository _repository;
        private readonly ILogger _logger;

        public BlobStorageForwarder(IMessageRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<string> Process(IParkingSpotStatusUpdate update)
        {
            _logger.LogInformation($"Processing spot {update.SpotId}.");

            return await _repository.Store(update);
        }
    }
}
