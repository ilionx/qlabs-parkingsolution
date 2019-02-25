using System;
using System.Collections.Generic;

namespace ProjectParking.Contracts
{
    public interface IParkingSpotStatusUpdate
    {
        int SpotId { get; }

        string Location { get; }

        bool Available { get; }

        DateTime Timestamp { get; }

        IDictionary<string, string> MetaData { get; }
    }
}
