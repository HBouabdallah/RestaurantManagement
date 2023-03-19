namespace RestaurantManagement
{
    public class GlobalSettings
    {
        public string connectionString { get; set; }
        public string logPath { get; set; }

        public static GlobalSettings GetSettings(IConfiguration configuration)
        {
            return new GlobalSettings()
            {
                connectionString = configuration.GetConnectionString("DBConnection").ToString(),
                logPath = configuration.GetConnectionString("LogPath").ToString()
            };
        }
    }
}
