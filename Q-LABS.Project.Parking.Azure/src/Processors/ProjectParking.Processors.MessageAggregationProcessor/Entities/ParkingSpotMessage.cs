using Microsoft.Azure.CosmosDB.Table;
using ProjectParking.Contracts;
using System;
using System.Collections.Generic;
using MassTransit;

namespace ProjectParking.Processors.MessageAggregationProcessor.Entities
{
    public class ParkingSpotMessage : TableEntity, IParkingSpotStatusUpdate
    {
        public ParkingSpotMessage()
        {

        }

        public ParkingSpotMessage(int spotId, string location)
        {
            this.PartitionKey = nameof(ParkingSpotMessage);
            this.RowKey = $"{NewId.NextGuid()}";
            this.SpotId = spotId;
            this.Location = location;
        }

        public int SpotId { get; set; }

        public string Location { get; set; }

        public bool Available { get; set; }

        public new DateTime Timestamp { get; set; }

        public IDictionary<string, string> MetaData { get; set; }

        public string MetaDataString { get; set; }
    }
}
