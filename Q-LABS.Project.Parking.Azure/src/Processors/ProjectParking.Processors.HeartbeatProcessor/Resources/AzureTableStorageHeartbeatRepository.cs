using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ProjectParking.Contracts;
using ProjectParking.Processors.HeartbeatProcessor.Entities;

namespace ProjectParking.Processors.HeartbeatProcessor.Resources
{
    internal class AzureTableStorageHeartbeatRepository : IHeartbeatRepository
    {
        private readonly CloudTable _heartbeats;
        private readonly ILogger _logger;

        public AzureTableStorageHeartbeatRepository(ILogger logger)
        {
            _logger = logger;

            //todo refactor to proper DI
            var storageAccount =
                CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"]
                    .ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();

            _heartbeats = tableClient.GetTableReference("heartbeats");
            _heartbeats.CreateIfNotExists();
        }

        public async Task Store(IParkingSpotStatusUpdate message)
        {
            await UpdateSpotStatus(message);
        }

        public async Task<UnresponsiveParkingSpot[]> RetrieveUnresponsiveParkingSpots()
        {
            var results = GetUnresponsiveSpots().ToArray();
            return ConvertToUnresponsiveParkingSpots(results);
        }

        public async Task MarkUnresponsiveSpots(params UnresponsiveParkingSpot[] spots)
        {
            foreach (var partition in spots.GroupBy(x => x.Location))
            {
                var operation = new TableBatchOperation();

                foreach (var parkingSpot in partition)
                {
                    var entity = ConvertToParkingSpotStatusUpdateEntity(parkingSpot);
                    entity.FailedToReply = true;
                    entity.ETag = "*";
                    operation.Add(TableOperation.Merge(entity));
                }

                await _heartbeats.ExecuteBatchAsync(operation);
            }
        }

        private async Task UpdateSpotStatus(IParkingSpotStatusUpdate message)
        {
            var entity = new ParkingSpotStatusUpdateEntity(message.SpotId, message.Location, message.Timestamp);

            var insertOperation = TableOperation.InsertOrReplace(entity);

            await _heartbeats.ExecuteAsync(insertOperation);
        }

        private static UnresponsiveParkingSpot[] ConvertToUnresponsiveParkingSpots(
            IEnumerable<ParkingSpotStatusUpdateEntity> results)
        {
            return results.Select(entity =>
                new UnresponsiveParkingSpot(entity.SpotId, entity.Location, entity.UpdateTimestamp)).ToArray();
        }

        private static ParkingSpotStatusUpdateEntity ConvertToParkingSpotStatusUpdateEntity(
            UnresponsiveParkingSpot input)
        {
            return new ParkingSpotStatusUpdateEntity(input.SpotId, input.Location, input.LastReceievedTimestamp);
        }

        private IEnumerable<ParkingSpotStatusUpdateEntity> GetUnresponsiveSpots()
        {
            var filter = TableQuery.CombineFilters(
                TableQuery.GenerateFilterConditionForDate(nameof(ParkingSpotStatusUpdateEntity.UpdateTimestamp),
                    QueryComparisons.LessThanOrEqual, DateTimeOffset.Now.AddMinutes(-1)),
                TableOperators.And,
                TableQuery.GenerateFilterConditionForBool(nameof(ParkingSpotStatusUpdateEntity.FailedToReply),
                    QueryComparisons.Equal, false));

            var query = new TableQuery<ParkingSpotStatusUpdateEntity>().Where(filter);

            var results = _heartbeats.ExecuteQuery(query);
            return results;
        }
    }
}