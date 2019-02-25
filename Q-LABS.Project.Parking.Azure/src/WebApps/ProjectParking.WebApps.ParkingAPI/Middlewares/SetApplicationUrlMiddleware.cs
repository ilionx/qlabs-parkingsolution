using Microsoft.AspNetCore.Http;
using ProjectParking.WebApps.ParkingAPI.BackgroundTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectParking.WebApps.ParkingAPI.Middlewares
{
    public class SetApplicationUrlMiddleware
    {
        private readonly RequestDelegate next;

        public SetApplicationUrlMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (String.IsNullOrEmpty(CarparkHubUpdateScheduledService.Server)) {
                var request = httpContext.Request;
                var absoluteUri = string.Concat(
                            request.Scheme,
                            "://",
                            request.Host.ToUriComponent(),
                            request.PathBase.ToUriComponent());
                CarparkHubUpdateScheduledService.Server = absoluteUri;
            }

            await next.Invoke(httpContext);
        }
    }
}
