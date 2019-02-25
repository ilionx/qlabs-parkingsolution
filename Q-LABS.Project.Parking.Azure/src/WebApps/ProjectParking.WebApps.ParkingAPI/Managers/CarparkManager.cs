using ProjectParking.Contracts.enumerables;
using ProjectParking.WebApps.ParkingAPI.DataTransferObjects;
using ProjectParking.WebApps.ParkingAPI.Entities;
using ProjectParking.WebApps.ParkingAPI.Providers.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProjectParking.WebApps.ParkingAPI.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public class CarparkManager
    {
        private readonly ICarparkProvider carparkProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carparkProvider"></param>
        public CarparkManager(ICarparkProvider carparkProvider) {
            this.carparkProvider = carparkProvider;
        }

        public IEnumerable<ParkingSpotDTO> GetParkingSpots(int carparkId) {

            Carpark carpark = GetCarparkById(carparkId);

            return carpark.SpotData.Select(x => new ParkingSpotDTO {
                LastUpdated = x.Value.LastUpdated,
                SpotId = x.Value.SpotId,
                Status = x.Value.SpotStatus,
                CarparkId = carparkId
            });
        }

        public IEnumerable<CarparkListDTO> GetCarparks() {
            return carparkProvider.GetCarparks().Select(carpark => new CarparkListDTO  {
                Id = carpark.Id,
                Name = carpark.Name,
                Location = new LocationDTO
                {
                    Latitude = carpark.Latitude,
                    Longitude = carpark.Longitude
                },
                AvailableParkingSpots = carpark.SpotData.Count(x => x.Value.SpotStatus == SpotStatus.Available)
            }).ToList();
        }

        public CarparkDTO GetCarpark(int carparkId) {
            Carpark carpark = GetCarparkById(carparkId);
            return new CarparkDTO
            {

                Name = carpark.Name,
                Description = carpark.Description,
                Location = new LocationDTO
                {
                    Latitude = carpark.Latitude,
                    Longitude = carpark.Longitude
                },
                Info = new CarparkInfoDTO
                {
                    AvailableSpaces = carpark.SpotData.Count(x => x.Value.SpotStatus == SpotStatus.Available),
                    UnavailableSpaces = carpark.SpotData.Count(x => x.Value.SpotStatus == SpotStatus.Unavailable),
                    UnknownSpaces = carpark.SpotData.Count(x => x.Value.SpotStatus == SpotStatus.Unknown),
                    TotalSpaces = 0,

                }
            };
        }

        private Carpark GetCarparkById(int carparkId) {
            Carpark carpark = carparkProvider.GetCarpark(carparkId);

            if (carpark == null)
            {
                throw new CarparkNotFoundException(carparkId);
            }
            return carpark;
        }
    }

    internal class CarparkNotFoundException : Exception {
        public CarparkNotFoundException(int carparkId) : base ($"There is no carpark with id: {carparkId}") { }
    }
}
