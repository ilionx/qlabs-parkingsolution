using Microsoft.AspNetCore.SignalR;
using ProjectParking.Contracts;
using ProjectParking.WebApps.ParkingAPI.DataTransferObjects;
using ProjectParking.WebApps.ParkingAPI.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectParking.WebApps.ParkingAPI.Hubs
{
    /// <summary>
    /// The hub used for broadcasting carpark information
    /// </summary>
    public class CarparkHub: Hub
    {
        private readonly CarparkManager carparkManager;

        public CarparkHub(CarparkManager carparkManager) {
            this.carparkManager = carparkManager;
        }

        public Task GetAllCarparks() {
            IEnumerable<CarparkListDTO> carparks = carparkManager.GetCarparks();
            return Clients.Caller.SendAsync("GetAllCarparks", carparks);

        }

        /// <summary>
        /// Send a list of current Carpark Data to all clients
        /// </summary>
        /// <param name="carparkId"></param>
        /// <param name="parkingSpots"></param>
        /// <returns></returns>
        public Task NotifyParkingStatusForCarpark(int carparkId, IEnumerable<ParkingSpotDTO> parkingSpots) =>
            Clients.All.SendAsync("NotifyParkingStatusForCarpark", carparkId, parkingSpots);

    }
}
