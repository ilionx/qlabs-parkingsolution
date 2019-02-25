using System.Threading.Tasks;
using ProjectParking.Contracts;

namespace ProjectParking.Processors.HeartbeatProcessor.Resources
{
    internal interface IHeartbeatRepository
    {
        Task Store(IParkingSpotStatusUpdate message);

        Task<UnresponsiveParkingSpot[]> RetrieveUnresponsiveParkingSpots();

        Task MarkUnresponsiveSpots(params UnresponsiveParkingSpot[] spots);
    }
}