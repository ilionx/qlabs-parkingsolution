using ProjectParking.Contracts;
using ProjectParking.Contracts.enumerables;
using ProjectParking.WebApps.ParkingAPI.Entities;
using ProjectParking.WebApps.ParkingAPI.Providers.Contracts;
using ProjectParking.WebApps.ParkingAPI.Providers.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectParking.WebApps.ParkingAPI.Providers.InMemory
{
    public class InMemoryCarparkProvider: ICarparkProvider
    {
        internal static Carparks Carparks = new Carparks();

        public IEnumerable<Carpark> GetCarparks() {
            return Carparks;
        }

        public Carpark GetCarpark(int id)
        {
            return Carparks.FirstOrDefault(x => x.Id == id);
        }

        public Carpark GetCarparkByName(string name)
        {
            Carpark carpark = Carparks.FirstOrDefault(x => x.Name == name);
            return carpark;

        }

        public void AddCarpark(Carpark carpark)
        {
            int id = Carparks.Any() ? Carparks.Max(x => x.Id) + 1 : 1;
            Carparks.Add(new Carpark {
                Id = id,
                Name = carpark.Name,
                Latitude = carpark.Latitude,
                Longitude = carpark.Longitude,
                TotalSpaces = carpark.TotalSpaces,
                Description = carpark.Description
            });
        }

        public void ProcessStatusUpdate(IParkingSpotStatusUpdate parkingSpotStatusUpdate)
        {
            Carpark carpark = GetCarparkByName(parkingSpotStatusUpdate.Location);
            if (carpark != null)
            {
                carpark.SpotData[parkingSpotStatusUpdate.SpotId] = new ParkingSpotImpl
                {
                    SpotId = parkingSpotStatusUpdate.SpotId,
                    LastUpdated = DateTime.Now,
                    SpotStatus = parkingSpotStatusUpdate.Available ? SpotStatus.Available : SpotStatus.Unavailable
                };

            }
        }

        public void ProcessSpotUpdate(ISpotUpdate spotUpdate)
        {
            Carpark carpark = GetCarpark(spotUpdate.CarparkId);
            carpark.SpotData[spotUpdate.SpotId] = new ParkingSpotImpl()
            {
                SpotId = spotUpdate.SpotId,
                LastUpdated = spotUpdate.UpdatedOn,
                SpotStatus = spotUpdate.Status
            };

        }

        public void CleanUp()
        {
            DateTime now = DateTime.Now;

            foreach (var carpark in Carparks)
            {

                List<int> unresponsiveSpots = carpark.SpotData.Where(x => (now - x.Value.LastUpdated).Seconds >= 20).Select(x => x.Key).ToList();

                if (unresponsiveSpots.Count > 0) {

                    foreach (int i in unresponsiveSpots) {
                        carpark.SpotData[i].SpotStatus = SpotStatus.Unknown;
                        carpark.SpotData[i].LastUpdated = now;
                    }
                }

                
            }
        }
    }
}
