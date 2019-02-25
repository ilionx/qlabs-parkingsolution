using System.Threading.Tasks;

namespace ProjectParking.Processors.HeartbeatProcessor.Resources
{
    internal interface INotificationAccess
    {
        Task SendNotificationFor(params UnresponsiveParkingSpot[] spot);
    }
}