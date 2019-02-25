using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using ProjectParking.WebApps.ParkingAPI.Entities;
using ProjectParking.WebApps.ParkingAPI.Providers.Contracts;
using System;
using System.Collections.Generic;

namespace ProjectParking.WebApps.ParkingAPI.Utilities
{
    public class CarparkSetupStartFilter : IStartupFilter
    {
        private readonly ICarparkProvider carparkProvider;
        private readonly IOptions<AppConfig> options;

        public CarparkSetupStartFilter(ICarparkProvider carparkProvider, IOptions<AppConfig> options) {
            this.carparkProvider = carparkProvider;
            this.options = options;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                IEnumerable<CarparkConfig> carparks = options.Value.Carparks;

                foreach (var carpark in carparks)
                {
                    carparkProvider.AddCarpark(new Carpark
                    {
                        Name = carpark.Name,
                        Latitude = carpark.Latitude,
                        Longitude = carpark.Longitude,
                        TotalSpaces = carpark.TotalSpots,
                        Description = carpark.Description
                    });
                }

                next(builder);
            };
        }
    }
}
