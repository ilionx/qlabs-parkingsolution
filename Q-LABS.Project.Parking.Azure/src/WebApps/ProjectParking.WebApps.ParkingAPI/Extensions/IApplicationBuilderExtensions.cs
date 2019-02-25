using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectParking.WebApps.ParkingAPI.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void ConfigureServiceBusLifetime(this IApplicationBuilder applicationBuilder, IApplicationLifetime applicationLifetime, IBusControl bus) {
            bus.StartAsync();

            applicationLifetime.ApplicationStopping.Register(() => bus.StopAsync());

        }
    }
}
