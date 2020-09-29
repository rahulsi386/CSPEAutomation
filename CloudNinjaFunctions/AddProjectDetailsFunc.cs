using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using CloudNinjaFunctions.Helpers;

namespace CloudNinjaFunctions
{
    public static class AddProjectDetailsFunc
    {
        private const string _cloudNinjaDBConnectionString = "CloudNinjaDB_ConString";
        private static readonly string CloudNinjaDBConString = Environment.GetEnvironmentVariable(_cloudNinjaDBConnectionString);

        [FunctionName("AddProjectDetailsFunc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Add Project details function invoked...");
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                log.LogInformation($"Project details to be added: {data}");

                string projname = data?.ProjectName;
                string projdesc = data?.Description;
                string owneremail = data?.ProjectOwnerEmail;
                string azservices = data?.AzureServicesUsed;
                string clarityid = data?.ClarityId;
                string ehlccd = data?.eHLCCD;
                string costcenter = data?.CostCenter;
                string billingcontact = data?.BillingContactEmail;
                DateTime createddate = data.CreatedDate;
                DateTime modifieddate = data.ModifiedDate;
                string createdby = data?.CreatedBy;
                string modifiedby = data?.ModifiedBy;

                log.LogInformation("Begin adding new row to dbo.projectinfo table");
                CloudNinjaDBOps.ExecuteInsertIntoProjectInfoTable(projname, projdesc, owneremail, azservices, clarityid, ehlccd, costcenter, billingcontact, createddate, modifieddate, createdby, modifiedby);
                log.LogInformation("Add project details function executed successfully.");
                return new OkObjectResult($"Status: {HttpStatusCode.OK} - Successfully added new project detail in database.");
            }
            catch(Exception ex)
            {
                log.LogInformation($"Error: {ex.Message} \nTrace: {ex.StackTrace}");
                return new OkObjectResult($"Status: {HttpStatusCode.BadRequest} - {ex.Message} - {ex.StackTrace}");
            }
           // IList<string> dbQueryResult = QueryProjectInfoTable();            
        }

       
    }
}
