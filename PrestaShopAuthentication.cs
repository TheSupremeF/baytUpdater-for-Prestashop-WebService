using System;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace APITesterPrestaShop
{
    /// <summary>
    /// Handles PrestaShop API authentication and request construction.
    /// </summary>
    public class PrestaShopAuthentication
    {  
        private static readonly PrestaShopAuthentication instance = new PrestaShopAuthentication();

        public string? PrestashopURL { get; private set; }
        public string? PrestashopEndpoint { get; private set; }
        private string? PrestashopAPIKey { get; set; }
        public string? PrestashopQueryFilter { get; private set; }

        private PrestaShopAuthentication()
        {
            LoadConfiguration();
        }

        public static PrestaShopAuthentication Instance => instance;

        /// <summary>
        /// Sets the endpoint for the API request.
        /// </summary>
        /// <param name="endpoint">The endpoint to set.</param>
        public void SetEndpoint(string endpoint) => PrestashopEndpoint = endpoint;

        /// <summary>
        /// Sets the query filter for the API request.
        /// </summary>
        /// <param name="queryFilter">The query filter to set.</param>
        public void SetQueryFilter(string queryFilter) => PrestashopQueryFilter = queryFilter;

        /// <summary>
        /// Combine URL for HTTP requests.
        /// </summary>
        /// <returns>The combined URL.</returns>
        public string GetUrl()
        {
            StringBuilder urlBuilder = new StringBuilder(PrestashopURL)
                .Append(PrestashopEndpoint)
                .Append("?filter[ean13]=[")
                .Append(PrestashopQueryFilter)
                .Append("]&io_format=JSON");

            return urlBuilder.ToString();
        }

        /// <summary>
        /// For debugging purposes, print Prestashop API Key, authentication header, and the URL.
        /// </summary>
        /// <param name="keyUrlShow">Indicates whether to show the API key and URL.</param>
        public void DebugLines()
        {
            Console.WriteLine("HTTP DEBUG MODE ON:");
            Console.WriteLine(new string('-', 10));
            Console.WriteLine($"API Key: {PrestashopAPIKey}");
            Console.WriteLine($"API URL: {PrestashopURL}");
            Console.WriteLine($"HTTP Request URL: {GetUrl()}");
            Console.WriteLine($"AuthHeader: {GetAuthHeader()}");
            Console.WriteLine(new string('-', 10));
            Console.WriteLine();

        }

        /// <summary>
        /// Since Prestashop uses Basic auth with only API Key as username, fit the API key to format.
        /// </summary>
        /// <returns>The formatted API key.</returns>
        private string GetApiKey() => $"{PrestashopAPIKey}:{""}";

        /// <summary>
        /// Set authentication headers for HTTP requests.
        /// </summary>
        /// <returns>The authentication header value.</returns>
        public AuthenticationHeaderValue GetAuthHeader() => new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(GetApiKey())));

        /// <summary>
        /// Load PrestaShopAuth settings from the configuration file.
        /// </summary>
        private void LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json");

            IConfigurationRoot configuration = builder.Build();

            PrestashopURL = configuration["PrestaShop Authentication:Prestashop URL"];
            EnsureUrlEndsWithApi();

            PrestashopAPIKey = configuration["PrestaShop Authentication:Prestashop API Key"];
        }    
             /// <summary>
             /// Ensures the Prestashop URL ends with "/api/".
             /// </summary>
        private void EnsureUrlEndsWithApi()
        {
            if (!string.IsNullOrEmpty(PrestashopURL) && !PrestashopURL.EndsWith("/api/"))
            {
                PrestashopURL += "/api/";
            }
        }
    }
}