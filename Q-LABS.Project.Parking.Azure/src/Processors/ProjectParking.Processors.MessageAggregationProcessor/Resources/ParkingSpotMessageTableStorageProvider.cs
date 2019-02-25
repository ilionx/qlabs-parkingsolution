using System.Configuration;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.Azure.Storage; // Namespace for StorageAccounts
using Microsoft.Azure.CosmosDB.Table; // Namespace for Table storage types
using ProjectParking.Contracts;
using Newtonsoft.Json;
using ProjectParking.Processors.MessageAggregationProcessor.Entities;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ProjectParking.Processors.MessageAggregationProcessor.Resources
{
    public class ParkingSpotMessageTableStorageProvider : IParkingSpotMessageProvider
    {
        private ILogger _logger;
        private CloudTable _messageTable;


        public ParkingSpotMessageTableStorageProvider(ILogger logger)
        {
            this._logger = logger;

            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            _messageTable = tableClient.GetTableReference("messages");
            _messageTable.CreateIfNotExists();
        }

        public async Task Store(IParkingSpotStatusUpdate parkingSpotStatusUpdate)
        {
            // Create a new customer entity.
            ParkingSpotMessage parkingSpot = new ParkingSpotMessage(parkingSpotStatusUpdate.SpotId, parkingSpotStatusUpdate.Location);
            parkingSpot.SpotId = parkingSpotStatusUpdate.SpotId;
            parkingSpot.Timestamp = parkingSpotStatusUpdate.Timestamp;
            parkingSpot.Available = parkingSpotStatusUpdate.Available;
            parkingSpot.MetaData = parkingSpotStatusUpdate.MetaData;
            parkingSpot.MetaDataString = JsonConvert.SerializeObject(parkingSpotStatusUpdate.MetaData);

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(parkingSpot);

            // Execute the insert operation.
            await _messageTable.ExecuteAsync(insertOperation);
        }
    }
}
