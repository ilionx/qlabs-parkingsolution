using ProjectParking.Contracts;
using System.Threading.Tasks;

namespace ProjectParking.Processors.MessageAggregationProcessor.Resources
{
    public interface IParkingSpotMessageProvider
    {
        Task Store(IParkingSpotStatusUpdate parkingSpotStatusUpdate);
    }
}
