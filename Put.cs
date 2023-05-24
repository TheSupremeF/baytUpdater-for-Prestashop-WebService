using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

class Put
{
    static async Task PutRequest(string[] args)
    {
        //Basic credentials
        string baseUrl = "prestashop.mustafaburakbesler.com/api/";
        string apiKey = "EJFCXV5QS3ID6PX1T751Q8ZJC8R5PJZR";
        
        
        string queryParameters = "?display=full&limit=10"; //Display queries 
        string jsonOutput = "&io_format=JSON"; // Json output queries

        string endPoint = "products"; // Endpoint to determine which data will be taken

        /*Create a HTTP Client */
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey+":"+"")));

        string interpolated = baseUrl + endPoint + queryParameters + jsonOutput;

        HttpContent httpContent = new StringContent("");
        HttpResponseMessage httpResponseMessage = await httpClient.PutAsync(interpolated,httpContent);

    }
}