namespace ProjectParking.WebApps.ParkingAPI.DataTransferObjects
{
    public class CarparkListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LocationDTO Location { get; set; }
        public int AvailableParkingSpots { get; set; }
    }
}
