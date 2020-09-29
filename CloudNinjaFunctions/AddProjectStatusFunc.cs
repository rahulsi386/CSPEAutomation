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
    public static class AddProjectStatusFunc
    {
        [FunctionName("AddProjectStatus")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Add Project status function invoked...");
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                log.LogInformation($"Project status details to be added: {data}");

                string projid = data?.proj_id;
                DateTime createddate = data.created_date;
                DateTime modifieddate = data.modified_date;
                string createdby = data?.created_by;
                string modifiedby = data?.modified_by;

                log.LogInformation("Begin adding new row to dbo.projectstatus table");
                CloudNinjaDBOps.ExecuteInsertIntoProjectStatusTable(projid, createddate, modifieddate, createdby, modifiedby);
                log.LogInformation("Add project status function executed successfully.");
                return new OkObjectResult($"Status: {HttpStatusCode.OK} - Successfully added new project status in database.");
            }
            catch (Exception ex)
            {
                log.LogInformation($"Error: {ex.Message} \nTrace: {ex.StackTrace}");
                return new OkObjectResult($"Status: {HttpStatusCode.BadRequest} - {ex.Message} - {ex.StackTrace}");
            }
        }
    }
}
