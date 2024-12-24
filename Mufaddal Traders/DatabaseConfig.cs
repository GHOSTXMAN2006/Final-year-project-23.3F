using System.Configuration;

namespace Mufaddal_Traders
{
    public static class DatabaseConfig
    {
        // Replace the connection string value with your actual one
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["MufaddalTradersDB"].ConnectionString;
    }
}
