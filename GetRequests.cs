using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace APITesterPrestaShop
{
    public class GetRequests
    {
        static readonly HttpClient httpClient = new HttpClient();

        static PrestaShopAuthentication prestaShopAuth = PrestaShopAuthentication.Instance;
        static DebugMode _debug = new DebugMode();

        public async Task<bool?> GetRequestAsync(string endpoint, string queryFilter)
        {
            prestaShopAuth.SetEndpoint(endpoint);
            prestaShopAuth.SetQueryFilter(queryFilter);

            DebugMode();

            // Set the authentication header on the HttpClient instance
            httpClient.DefaultRequestHeaders.Authorization = prestaShopAuth.GetAuthHeader();
            // Send the API request and get the response
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(prestaShopAuth.GetUrl());
                HttpContent httpContent = response.Content;
                string responseContent = await httpContent.ReadAsStringAsync();
                int productId;
                
                if (response.StatusCode is HttpStatusCode.OK)
                {
                    // Print the response status code
                    Console.WriteLine("Response: HTTP 200 OK ");
                    if (responseContent != "[]")
                    {
                        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        ProductResponse? products = JsonSerializer.Deserialize<ProductResponse>(responseContent, jsonSerializerOptions);

                        if (products != null && products.Products?.Count > 0)
                        {
                            productId = products.Products[0].Id;
                            Console.WriteLine($"Product barcode: {prestaShopAuth.PrestashopQueryFilter}");
                            Console.WriteLine($"Product ID: {products.Products[0].Id}");
                            
                            return true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No products found.");
                        return false;
                    }

                }
                else
                {
                    Console.WriteLine($"Response status code: {(int)response.StatusCode} {response.ReasonPhrase}");
                    try
                    {
                        if (response.StatusCode is HttpStatusCode.Unauthorized)
                        {
                            Console.WriteLine();
                            throw new Exception("Exception: Unauthorized login.");
                        }
                        else if (response.StatusCode is HttpStatusCode.NotFound)
                        {
                            Console.WriteLine();
                            throw new Exception("Exception: Couldn't find the server.");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine();
                        Console.WriteLine(e.Message);
                        Console.Write(" Press a key to exit.");
                        Console.ReadKey();
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            return null;
        }

        private void DebugMode()
        {
            if (_debug.Debug)
            {
                // Print the authentication header and the URL
                prestaShopAuth.DebugLines();
            }
        }

        internal class ProductResponse
        {
            public List<Product>? Products { get; set; }
        }

        internal class Product
        {
            public int Id { get; set; }
        }
    }
}