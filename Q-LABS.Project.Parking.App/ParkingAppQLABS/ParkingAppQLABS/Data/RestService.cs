using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ParkingAppQLABS.Models;
using Newtonsoft.Json;
using ParkingAppQLABS.Helpers;

namespace ParkingAppQLABS.Data
{
    public class RestService : IRestService
    {
        private HttpClient _client;

        private string _parkingUrl;
        private string _parkingApiKey = Settings.ParkingApiKey;
        private string _parkingApiHeader = Settings.ParkingApiHeader;
        private string _raspberryIP = Settings.RaspberrySettings; //the pi's ip
        private string _carsUrl = "http://"; //url used to communicate with the pi

        public RestService()
        {
            _client = new HttpClient {MaxResponseContentBufferSize = 256000};
            _client.DefaultRequestHeaders.Add(_parkingApiHeader, _parkingApiKey);

            _parkingUrl = Settings.TestMode ? Settings.ParkingUrlTest : Settings.ParkingUrl;
        }

        public async Task<LinkInfoWrapper_List_LinkInfoWrapper_CarparkListDTO> GetCarParksAsync()
        {
            var uri = new Uri(_parkingUrl);
            var convertedContent = new LinkInfoWrapper_List_LinkInfoWrapper_CarparkListDTO()
            {
                value = new List<LinkInfoWrapper_CarparkListDTO>()
                {
                    //new LinkInfoWrapper_CarparkListDTO() {value = new CarparkListDTO() {availableParkingSpots = 0, name = "QNH"}},
                    //new LinkInfoWrapper_CarparkListDTO() {value = new CarparkListDTO() {availableParkingSpots = 1, name = "JAGUAR"}},
                    //new LinkInfoWrapper_CarparkListDTO() {value = new CarparkListDTO() {availableParkingSpots = 2, name = "PLATO"}},
                    //new LinkInfoWrapper_CarparkListDTO() {value = new CarparkListDTO() {availableParkingSpots = 3, name = "ENGIE"}},
                    //new LinkInfoWrapper_CarparkListDTO() {value = new CarparkListDTO() {availableParkingSpots = 5, name = "TEST"}}
                }
            };

            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    convertedContent = JsonConvert.DeserializeObject<LinkInfoWrapper_List_LinkInfoWrapper_CarparkListDTO>(content);
                }
            }
            catch (Exception e)
            {
                //log
            }

            return convertedContent;
        }

        public async Task<List<string>> GetCarsAsync()
        {
            if (Uri.IsWellFormedUriString(_carsUrl + _raspberryIP + ":5000", UriKind.Absolute))
            {
                var uri = new Uri(_carsUrl + _raspberryIP + ":5000");

                try
                {
                    var response = await _client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        List<string> convertedContent = JsonConvert.DeserializeObject<List<string>>(content);
                        return convertedContent;
                    }
                }
                catch (Exception e)
                {
                    //log
                }
            }
            return null;
        }
    }
}
