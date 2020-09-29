using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CloudNinjaBot.Helpers
{
    public class UploadDocToBlob
    {
        //private const string _functionUrl = "AddProjectDetailsFunc_URL";
        //private static readonly string functionEndpoint = Environment.GetEnvironmentVariable(_functionUrl);

        //private const string _functionKey = "AddProjectDetailsFunc_API_KEY";
        //private static readonly string functionApiKey = Environment.GetEnvironmentVariable(_functionKey);
       
        //Below four lines of code is only for testing during development
        private const string _functionUrl = "https://cloudninjafunctions.azurewebsites.net/api/UploadDocToBlobFunc";
        private static readonly string functionEndpoint = _functionUrl;
        private const string _functionKey = "LKDhbtAelQ9O2bTnHhY7kBF2P7/ilxxdlFybcSzX/faOc/B55OqTZQ==";
        private static readonly string functionApiKey = _functionKey;

        public static async Task<string> InvokeUploadDocToBlobFunction(string fileName, string filePath)
        {
            var uploadDoc = new UploadDoc
            {
                FileName=fileName,
                FilePath=filePath
            };
            var requestBody = JsonConvert.SerializeObject(uploadDoc);
            //var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    // Build the request.
                    // Set the method to Post.
                    request.Method = HttpMethod.Post;
                    // Construct the URI and add headers.
                    request.RequestUri = new Uri(functionEndpoint);
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Headers.Add("X-Functions-Key", functionApiKey);

                    // Send the request and get response.
                    HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);
                    // Read response as a string.
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(result);
                    Console.ReadLine();
                    return result;
                }
            }
        }
    }

    public class UploadDoc
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
