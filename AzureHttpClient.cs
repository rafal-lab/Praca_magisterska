using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;

namespace xxx
{
    public class AzureRetailPricesApiClient
    {
        private const int RetryDelayMilliseconds = 1000; // Opóźnienie między żądaniami w milisekundach
        private const string BaseUrl = "https://prices.azure.com/api/retail/prices";


        public async Task<AzureRetailPricesApiResponse> GetPricesAsync(string serviceName, string meterName, string productName, string nextPageLink = null)
        {
            string queryString = $"?";
            if (serviceName == "Virtual Machines" || serviceName == "Azure Firewall" || serviceName == "Storage" || serviceName == "ExpressRoute")
            {
                queryString = $"?$filter=serviceName eq '{serviceName}' and location eq 'EU West' and productName eq '{productName}' and meterName eq '{meterName}'";
            }
            else if(productName == "SQL Database Single/Elastic Pool General Purpose - SQL License" || serviceName == "Load Balancer")
            {
                queryString = $"?$filter=serviceName eq '{serviceName}' and location eq 'Global' and productName eq '{productName}' and meterName eq '{meterName}'";
            }
            else if(productName == "SQL Database Single/Elastic Pool General Purpose - Storage")
            {
                queryString = $"?$filter=serviceName eq '{serviceName}' and location eq 'EU West' and productName eq '{productName}' and meterName eq '{meterName}' and meterId eq '62abdcb7-13bf-4ab6-98af-d22be94f9081'";
            }
            else if(serviceName == "Azure Cosmos DB")
            {
                queryString = $"?$filter=serviceName eq '{serviceName}' and location eq 'EU West' and productName eq '{productName}' and meterName eq '{meterName}'";

            }
            else if (serviceName == "SQL Database")
            {
                queryString = $"?$filter=serviceName eq '{serviceName}' and location eq 'EU West' and productName eq '{productName}' and skuName eq '{meterName}'";

            }
            else if(serviceName == "Azure DNS")
            {
                queryString = $"?$filter=serviceName eq '{serviceName}' and location eq 'Zone 1' and productName eq '{productName}' and meterName eq '{meterName}'";

            }


            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);

                try
                {
                    if (!string.IsNullOrEmpty(nextPageLink))
                    {
                        httpClient.BaseAddress = new Uri(nextPageLink);
                    }
                   //Debug.WriteLine(httpClient.BaseAddress);
                   //Debug.WriteLine(queryString);
                    HttpResponseMessage response = await httpClient.GetAsync(queryString);
                    if (response.StatusCode == (HttpStatusCode)429)
                    {
                        // Opóźnienie przed ponownym wysłaniem żądania w przypadku kodu stanu 429 (Too Many Requests)
                        await Task.Delay(RetryDelayMilliseconds);
                        return await GetPricesAsync(serviceName, meterName, nextPageLink);
                    }
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    //Debug.WriteLine($"API logs: content is {content}"); // log the API response

                    var azureRetailPricesApiResponse = JsonConvert.DeserializeObject<AzureRetailPricesApiResponse>(content);

                    return azureRetailPricesApiResponse;
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception($"AzureHttpClient Failed to retrieve Azure Retail Prices. {ex.Message}");
                }
            }
        }

        public async Task<List<AzureRetailPrice>> GetAllPricesAsync(string serviceName,  string meterName, string productName)
        {
            var allPrices = new List<AzureRetailPrice>();
            string nextPageLink = null;

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);

                do
                {
                    var apiResponse = await GetPricesAsync(serviceName, meterName, productName, nextPageLink);
                    allPrices.AddRange(apiResponse.Items);

                    nextPageLink = apiResponse.NextPageLink;
                } while (!string.IsNullOrEmpty(nextPageLink));
            }

            return allPrices;
        }




    }

    public class AzureRetailPricesApiResponse
    {
        public AzureRetailPrice[] Items { get; set; }
        public string NextPageLink { get; set; }
    }

    public class AzureRetailPrice
    {
        public string Id { get; set; }
        public string CurrencyCode { get; set; }
        public string Name { get; set; }
        public string Tier { get; set; }
        public double RetailPrice { get; set; }
        public string UnitOfMeasure { get; set; }
        public string ServiceName { get; set; }
        public string ServiceId { get; set; }
        public string Location { get; set; }
        public string MeterId { get; set; }
        public string MeterName { get; set; }
        public string MeterCategory { get; set; }
        public string MeterSubCategory { get; set; }
        public string UnitPrice { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public double TierMinimumUnits { get; set; }
        public string ArmRegionName { get; set; }
        public string ProductId { get; set; }
        public string SkuId { get; set; }
        public string ProductName { get; set; }
        public string SkuName { get; set; }
        public string ServiceFamily { get; set; }
        public string Type { get; set; }
        public bool IsPrimaryMeterRegion { get; set; }
        public string ArmSkuName { get; set; }
    }



}
