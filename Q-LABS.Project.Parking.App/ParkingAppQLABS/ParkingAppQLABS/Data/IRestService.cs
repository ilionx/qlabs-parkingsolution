using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ParkingAppQLABS.Models;

namespace ParkingAppQLABS.Data
{
    public interface IRestService
    {
        Task<LinkInfoWrapper_List_LinkInfoWrapper_CarparkListDTO> GetCarParksAsync();
        Task<List<string>> GetCarsAsync();
    }
}
