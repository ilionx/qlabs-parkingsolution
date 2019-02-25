using Microsoft.AspNetCore.Builder;

namespace ProjectParking.WebApps.ParkingAPI.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }

        public static IApplicationBuilder UseApplicationUrlMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SetApplicationUrlMiddleware>();
        }
    }
}
