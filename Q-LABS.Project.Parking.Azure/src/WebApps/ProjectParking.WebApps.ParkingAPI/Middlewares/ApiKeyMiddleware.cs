using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using ProjectParking.WebApps.ParkingAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectParking.WebApps.ParkingAPI.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string apiKey;

        public ApiKeyMiddleware(RequestDelegate next, IOptions<AppConfig> config) {
            this.next = next;
            this.apiKey = config.Value.ApiKey;
        }

        public async Task Invoke(HttpContext httpContext) {
            try
            {
                var remoteIpAddress = httpContext.Connection.RemoteIpAddress;
                var path = httpContext.Request.Path;

                if (path.StartsWithSegments("/api")) {

                    StringValues requestApiKey;
                    httpContext.Request.Headers.TryGetValue(AppConstants.API_KEY_HEADER, out requestApiKey);

                    if (requestApiKey.Count == 0)
                    {
                        httpContext.Response.StatusCode = 400; // Bad Request;
                        await httpContext.Response.WriteAsync("API Key is missing in the request");
                        return;
                    }
                    else if (!apiKey.Equals(requestApiKey.First())) {
                        httpContext.Response.StatusCode = 401; // Bad Request;
                        await httpContext.Response.WriteAsync("API Key is not valid");
                        return;
                    }

                }

                await next.Invoke(httpContext);
            }
            catch (Exception e) {
                throw;
            }
        }
    }
}
