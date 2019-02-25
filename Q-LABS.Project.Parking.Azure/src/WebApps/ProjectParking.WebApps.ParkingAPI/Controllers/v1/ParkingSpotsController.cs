using Microsoft.AspNetCore.Mvc;
using ProjectParking.WebApps.ParkingAPI.DataTransferObjects;
using ProjectParking.WebApps.ParkingAPI.Entities;
using ProjectParking.WebApps.ParkingAPI.Extensions.Hateoas;
using ProjectParking.WebApps.ParkingAPI.Extensions.Paging;
using ProjectParking.WebApps.ParkingAPI.Managers;
using ProjectParking.WebApps.ParkingAPI.Providers.Contracts;
using ProjectParking.WebApps.ParkingAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectParking.WebApps.ParkingAPI.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/Carparks/{carparkId}/[controller]")]
    public class ParkingSpotsController : Controller
    {
        private readonly CarparkManager carparkManager;
        private readonly IUrlHelper urlHelper;

        public ParkingSpotsController(CarparkManager carparkManager, IUrlHelper urlHelper) {
            this.carparkManager = carparkManager;
            this.urlHelper = urlHelper;
        }


        [HttpGet(Name = AppConstants.ApiMethods.ParkingSpots.GetForCarpark)]
        public LinkInfoWrapper<List<LinkInfoWrapper<ParkingSpotDTO>>> Get(int carparkId, int pageNumber = 0, int pageSize = 10) {

            var pagedResult = new PagedResult<LinkInfoWrapper<ParkingSpotDTO>>(carparkManager.GetParkingSpots(carparkId).Select(x => mapToParkingSpotDTO(x, carparkId)).AsQueryable(), pageNumber, pageSize);

            return pagedResult.BuildLinks(urlHelper, AppConstants.ApiMethods.ParkingSpots.GetForCarpark);
        }

        private LinkInfoWrapper<ParkingSpotDTO> mapToParkingSpotDTO(ParkingSpotDTO parkingSpot, int carparkId) {
            return new LinkInfoWrapper<ParkingSpotDTO> {
                Value = parkingSpot,
                Links = new List<LinkInfo> {
                    new LinkInfo {
                        Href = urlHelper.Link(AppConstants.ApiMethods.Carparks.GetAll, new {id = carparkId}),
                        Method = "GET",
                        Rel = "carpark"
                    }
                }
            };
        }

    }
}
