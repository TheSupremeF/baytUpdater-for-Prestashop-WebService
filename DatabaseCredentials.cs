using System;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace APITesterPrestaShop
{
    /// <summary>
    /// Handles database connection string creation.
    /// </summary>
    public class DatabaseCredentials
    {
        private static readonly DatabaseCredentials instance = new DatabaseCredentials();

        public string? ServerName { get; private set; }
        public string? DatabaseName { get; private set; }
        public string? Username { get; private set; }
        public string? Password { get; private set; }

        private DatabaseCredentials()
        {
            LoadConfiguration();
        }

        /// <summary>
        /// Singleton instance for one-time usage.
        /// </summary>
        public static DatabaseCredentials Instance => instance;

        /// <summary>
        /// Combines the credentials into a single connection string for .NET database connections.
        /// </summary>
        /// <returns>The constructed connection string.</returns>
        public string GetConnectionString()
        {
            StringBuilder connectionStringBuilder = new StringBuilder()
                .Append("Data Source=").Append(ServerName).Append(";")
                .Append("Initial Catalog=").Append(DatabaseName).Append(";")
                .Append("User ID=").Append(Username).Append(";")
                .Append("Password=").Append(Password);

            return connectionStringBuilder.ToString();
        }

        /// <summary>
        /// Loads database credentials from the configuration file.
        /// </summary>
        private void LoadConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json");

            IConfigurationRoot configuration = builder.Build();

            ServerName = configuration["Database Credentials:Server Name"];
            DatabaseName = configuration["Database Credentials:Database Name"];
            Username = configuration["Database Credentials:Username"];
            Password = configuration["Database Credentials:Password"];

            ValidateConfiguration();
        }

        /// <summary>
        /// Validates the loaded configuration values.
        /// </summary>
        private void ValidateConfiguration()
        {
            try
            {
                if (string.IsNullOrEmpty(ServerName) ||
                    string.IsNullOrEmpty(DatabaseName) ||
                    string.IsNullOrEmpty(Username) ||
                    string.IsNullOrEmpty(Password))
                {
                    throw new InvalidOperationException("One or more database credentials are missing or invalid in the configuration file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
