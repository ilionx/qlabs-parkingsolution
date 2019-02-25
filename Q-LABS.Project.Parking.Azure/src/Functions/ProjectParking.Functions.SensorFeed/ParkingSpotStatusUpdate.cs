using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ProjectParking.Contracts;

namespace ProjectParking.Functions.SensorFeed
{

    public class MassTransitEnvelope
    {
        public string destinationAddress { get; set; }
        public Headers headers { get; set; }
        public ParkingSpotStatusUpdate message { get; set; }
        public string[] messageType { get; set; }
    }

    public class Headers
    {
    }


    public class ParkingSpotStatusUpdate : IParkingSpotStatusUpdate
    {
        [JsonProperty("spotId")]
        public int SpotId { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("available")]
        public bool Available { get; set; }

        public DateTime Timestamp { get; } = DateTime.Now;

        [JsonProperty("metadata")]
        public IDictionary<string, string> MetaData { get; set; }
    }
}
