using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CloudNinja.Helpers
{
    public class AddProjectDetails
    {

        //private const string _functionUrl = "AddProjectDetailsFunc_URL";
        //private static readonly string functionEndpoint = Environment.GetEnvironmentVariable(_functionUrl);

        //private const string _functionKey = "AddProjectDetailsFunc_API_KEY";
        //private static readonly string functionApiKey = Environment.GetEnvironmentVariable(_functionKey);

        //Below four lines of code is only for testing during development
        private const string _functionUrl = "https://cloudninjafunctions.azurewebsites.net/api/AddProjectDetailsFunc";
        private static readonly string functionEndpoint = _functionUrl;
        private const string _functionKey = "JNcvCijKaLaE8Ba7gs7tmTzboIBqf3/oXg0l/BCofUpYFIMYqOGV/A==";
        private static readonly string functionApiKey = _functionKey;

        public static async Task<string> InvokeAddProjectDetailsFunction(
            string name, string desc, string owneremail, string azservices, string clarityid,
            string ehlccd, string costcenter, string billingcontact)
        {
            var projectInfo = new ProjectInfo
            {
                ProjectName = name,
                Description = desc,
                ProjectOwnerEmail = owneremail,
                AzureServicesUsed = azservices,
                ClarityId = clarityid,
                eHLCCD = ehlccd,
                CostCenter = costcenter,
                BillingContactEmail = billingcontact,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                CreatedBy= "CloudNinjaBot",
                ModifiedBy= "CloudNinjaBot"

            };
            var requestBody = JsonConvert.SerializeObject(projectInfo);
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
                    return result;
                }
            }
        }
    }
}
