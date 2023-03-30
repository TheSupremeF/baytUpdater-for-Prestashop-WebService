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


/*
public class PrestaShopAuth
{
    public string PrestashopApiUrl { get; }
    public string PrestashopEndpoint { get; }
    public string PrestashopApiKey { get; }
    public bool PrestashopQueryHasEan13 { get; }
    public string PrestashopQueryFilter { get; }

    public PrestaShopAuth(string prestashopApiUrl, string prestashopEndpoint, string prestashopApiKey, string prestashopQueryFilter, bool prestashopQueryHasEan13 = false)
    {
        PrestashopApiUrl = prestashopApiUrl;
        PrestashopEndpoint = prestashopEndpoint;
        PrestashopApiKey = prestashopApiKey;
        PrestashopQueryHasEan13 = prestashopQueryHasEan13;
        PrestashopQueryFilter = prestashopQueryFilter;
    }


    public string GetUrl()
    {
        string endPointcombiner = $"{PrestashopApiUrl}{PrestashopEndpoint}";
        if (PrestashopQueryHasEan13)
        {
            return $"{endPointcombiner}?filter[ean13]=[{PrestashopQueryFilter}]&io_format=JSON";

        }
        else
        {
            return $"{endPointcombiner}?filter[reference]=[{PrestashopQueryFilter}]&io_format=JSON";
        }
    }


    public string GetApiKey() => $"{PrestashopApiKey}:{""}";

    public AuthenticationHeaderValue GetAuthHeader() => new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(GetApiKey())));

}*/