namespace ProjectParking.WebApps.ParkingAPI
{
    public static class AppConstants
    {
        public static readonly string API_KEY_HEADER = "X-API-KEY";

        public struct ApiMethods {

            public struct Carparks {
                public const string GetAll = "GetCarparks";
                public const string GetById = "GetCarparkDetails";
            }

            public struct ParkingSpots {
                public const string GetForCarpark = "ParkingSpotsForCarpark";
                public const string GetByIdForCarpark = "GetByIdForCarPark";

            }

        }
    }
}
