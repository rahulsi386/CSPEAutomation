using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CloudNinjaFunctions.Helpers;
using Newtonsoft.Json;

namespace CloudNinjaBot.Helpers
{
    public class AddProjectStatus
    {
        //private const string _functionUrl = "AddProjectDetailsFunc_URL";
        //private static readonly string functionEndpoint = Environment.GetEnvironmentVariable(_functionUrl);

        //private const string _functionKey = "AddProjectDetailsFunc_API_KEY";
        //private static readonly string functionApiKey = Environment.GetEnvironmentVariable(_functionKey);
       
        //Below four lines of code is only for testing during development
        private const string _functionUrl = "https://cloudninjafunctions.azurewebsites.net/api/AddProjectStatus";
        private static readonly string functionEndpoint = _functionUrl;
        private const string _functionKey = "xwRZDSCkb199Udu1kLNIT8m7cVv9m3UgalWvvQbBmzEHKgGah5CwMQ==";
        private static readonly string functionApiKey = _functionKey;

        public static async Task<string> InvokeAddProjectStatusFunction(Guid projid)
        {
            var projectStatus = new ProjectStatus
            {
                proj_id = projid,
                created_date = DateTime.Now,
                modified_date = DateTime.Now,
                created_by = "CloudNinja Bot",
                modified_by = "CloudNinja Bot"

            };
            var requestBody = JsonConvert.SerializeObject(projectStatus);
            //var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage();
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
            return result;
        }

    }
}
