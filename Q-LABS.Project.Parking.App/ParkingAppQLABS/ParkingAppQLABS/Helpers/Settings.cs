using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ParkingAppQLABS.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        private const string RaspberryKey = "raspberry_key";
        private static readonly string RaspberryDefault = string.Empty;

        private const string CarKey = "car_key";
        private static readonly string CarDefault = string.Empty;

        private const string DebugKey = "debug_key";
        private static readonly bool DebugDefault = false;

        private const string TestKey = "test_key";
        private static readonly bool TestDefault = false;
        #endregion


        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }

        public static string RaspberrySettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(RaspberryKey, RaspberryDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(RaspberryKey, value);
            }
        }

        public static string CarSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(CarKey, CarDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(CarKey, value);
            }
        }

        public static bool DebugMode
        {
            get
            {
                return AppSettings.GetValueOrDefault(DebugKey, DebugDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(DebugKey, value);
            }
        }

        public static bool TestMode
        {
            get
            {
                return AppSettings.GetValueOrDefault(TestKey, TestDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(TestKey, value);
            }
        }

        public static string ParkingUrlTest = ""; //test
        public static string ParkingUrl = ""; //prod
        public static string ParkingApiKey = "";
        public static string ParkingApiHeader = "";
        public static string SettingsPassword = "Qlabs2018"; //Password to change settings
    }
}

