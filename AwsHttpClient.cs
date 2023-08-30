using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace xxx
{
    public class AwsPricingApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://pricing.us-east-1.amazonaws.com";

        public AwsPricingApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<string> GetPricesAsync(string service = null, string region = null, string instanceType = null, string os = null, string term = null)
        {
            var queryString = "/offers/v1.0/aws/index.json";

            // Filter by service
            if (!string.IsNullOrEmpty(service))
            {
                queryString += $"?filters=productFamily:{service}";
            }

            // Filter by region
            if (!string.IsNullOrEmpty(region))
            {
                if (queryString.Contains("?"))
                {
                    queryString += ";";
                }
                else
                {
                    queryString += "?";
                }
                queryString += $"region={region}";
            }

            // Filter by instance type
            if (!string.IsNullOrEmpty(instanceType))
            {
                if (queryString.Contains("?"))
                {
                    queryString += ";";
                }
                else
                {
                    queryString += "?";
                }
                queryString += $"instanceType={instanceType}";
            }

            // Filter by operating system
            if (!string.IsNullOrEmpty(os))
            {
                if (queryString.Contains("?"))
                {
                    queryString += ";";
                }
                else
                {
                    queryString += "?";
                }
                queryString += $"operatingSystem={os}";
            }

            // Filter by term
            if (!string.IsNullOrEmpty(term))
            {
                if (queryString.Contains("?"))
                {
                    queryString += ";";
                }
                else
                {
                    queryString += "?";
                }
                queryString += $"term={term}";
            }

            var response = await _httpClient.GetAsync(queryString);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to retrieve AWS pricing. StatusCode={response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }
    }
}
