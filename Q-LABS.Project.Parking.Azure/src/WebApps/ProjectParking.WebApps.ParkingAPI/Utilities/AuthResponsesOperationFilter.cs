using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProjectParking.WebApps.ParkingAPI.Utilities
{
    public class AuthResponsesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            bool apiController = context.ApiDescription.RelativePath.StartsWith("api");
            if (apiController) {
                operation.Responses.Add("400", new Response { Description = "API Key is missing in the request" });
                operation.Responses.Add("401", new Response { Description = "API Key is not valid" });
            }
        }
    }
}
