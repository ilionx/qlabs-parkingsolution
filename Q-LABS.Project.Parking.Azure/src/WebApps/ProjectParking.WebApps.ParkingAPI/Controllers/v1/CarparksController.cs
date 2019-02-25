using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectParking.WebApps.ParkingAPI.DataTransferObjects;
using ProjectParking.WebApps.ParkingAPI.Entities;
using ProjectParking.WebApps.ParkingAPI.Extensions.Hateoas;
using ProjectParking.WebApps.ParkingAPI.Extensions.Paging;
using ProjectParking.WebApps.ParkingAPI.Managers;
using ProjectParking.WebApps.ParkingAPI.Providers.Contracts;
using ProjectParking.WebApps.ParkingAPI.Utilities;

namespace ProjectParking.WebApps.ParkingAPI.Controllers.v1
{
    /// <summary>
    /// Carpark API V1
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class CarparksController : Controller
    {
        private readonly CarparkManager carparkManager;
        private readonly IUrlHelper urlHelper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="carparkManager"></param>
        public CarparksController(IUrlHelper urlHelper, CarparkManager carparkManager)
        {
            this.urlHelper = urlHelper;
            this.carparkManager = carparkManager;
        }

        /// <summary>
        /// Retrieve a paginated list of carparks
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <response code="200">Retrieved Carparks</response>
        [HttpGet(Name = AppConstants.ApiMethods.Carparks.GetAll)]
        [ProducesResponseType(typeof(LinkInfoWrapper<List<LinkInfoWrapper<CarparkListDTO>>>), 200)]
        public LinkInfoWrapper<List<LinkInfoWrapper<CarparkListDTO>>> Get(int pageNumber = 0, int pageSize = 10)
        {
            return new PagedResult<LinkInfoWrapper<CarparkListDTO>>(this.carparkManager.GetCarparks().Select(res => MapToCarparkListDTO(res)).AsQueryable(), pageNumber, pageSize)
                .BuildLinks(urlHelper, "GetCarparks");
        }

        /// <summary>
        /// Retrieve a single carpark's details
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Carpark</response>
        /// <response code="404">Not existing carpark</response>
        [HttpGet("{id}", Name = AppConstants.ApiMethods.Carparks.GetById)]
        [ProducesResponseType(typeof(LinkInfoWrapper<CarparkDTO>), 200)]
        [ProducesResponseType(404)]
        public IActionResult Get(int id)
        {
            try
            {
                CarparkDTO carpark = carparkManager.GetCarpark(id);
                return Ok(MapToCarparkDTO(carpark, id));
            }
            catch (CarparkNotFoundException cnfe) {
                return NotFound(cnfe.Message);
            }

        }

        private LinkInfoWrapper<CarparkDTO> MapToCarparkDTO(CarparkDTO carpark, int id)
        {
            return new LinkInfoWrapper<CarparkDTO>
            {
                Value = carpark,
                Links = new List<LinkInfo> {
                    new LinkInfo {
                        Href = urlHelper.Link("Get", new { id = id }),
                        Method = "GET",
                        Rel = "self"
                    },
                    new LinkInfo {
                        Href = urlHelper.Link(AppConstants.ApiMethods.ParkingSpots.GetForCarpark, new {carParkId = id}),
                        Method = "GET",
                        Rel = "parkingspots"
                    }
                }
            };
        }

        private LinkInfoWrapper<CarparkListDTO> MapToCarparkListDTO(CarparkListDTO carpark) {
            return new LinkInfoWrapper<CarparkListDTO>
            {
                Value = carpark,
                Links = new List<LinkInfo> {
                    new LinkInfo {
                        Href = urlHelper.Link(AppConstants.ApiMethods.Carparks.GetById, new { id = carpark.Id }),
                        Method = "GET",
                        Rel = "self"
                    }
                }
            };
        } 

    }

}
