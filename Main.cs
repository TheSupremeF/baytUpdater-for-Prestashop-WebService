﻿using APITesterPrestaShop;
public class Program
{
    static GetRequests getRequest = new GetRequests();
    static SQLDataFetch sqlDataFetch = new SQLDataFetch();

    static async Task Main(string[] args)
    {
        await sqlDataFetch.SQLDataFetchAsync(DatabaseCredentials.Instance.GetConnectionString());
        Console.WriteLine();

        bool? productFound = await getRequest.GetRequestAsync(
            endpoint: "products",
            queryFilter: "869239349201"
            );

        if (productFound == false)
        {
            // Perform desired actions when the product is not found
        }
    }
}
