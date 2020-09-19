using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Identity;
using System.Data.SqlClient;
using Azure.Core;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

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
                //string projname = req.Query["name"];
                //string projdesc = req.Query["desc"];
                //string owneremail = req.Query["owneremail"];
                //string azservices = req.Query["azservices"];
                //string clarityid = req.Query["clarityid"];
                //string ehlccd = req.Query["ehlccd"];
                //string costcenter = req.Query["costcenter"];
                //string billingcontact = req.Query["billingcontact"];
                //DateTime createddate = DateTime.ParseExact(req.Query["createddate"], "yyyy-MM-ddHH:mm:ss", CultureInfo.InvariantCulture);
                //DateTime modifieddate = DateTime.ParseExact(req.Query["modifieddate"], "yyyy-MM-ddHH:mm:ss", CultureInfo.InvariantCulture);
                //string createdby = req.Query["createdby"];
                //string modifiedby = req.Query["modifiedby"];

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
                ExecuteTSqlNonQuery(projname, projdesc, owneremail, azservices, clarityid, ehlccd, costcenter, billingcontact, createddate, modifieddate, createdby, modifiedby);
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

        private static string GetTokenForCloudNinjaDb()
        {
            var credential = new DefaultAzureCredential();
            var tokenRequestContext = new TokenRequestContext(new[] { "https://database.windows.net/.default" });
            var accessToken = credential.GetTokenAsync(tokenRequestContext).Result.Token.ToString();

            return accessToken;
        }

        public static string InsertIntoProjectInfoTable()
        {
            return @"
               INSERT INTO dbo.ProjectInfo 
               (
                Proj_Id, Proj_Name,Proj_Description,Owner_Email,
                Azure_Services_Used,Clarity_Id,eHLCCD_Id,Cost_Center,
                Billing_Contact_Email,Created_Date,Modified_Date, 
                Created_By,Modified_By
               ) 
               VALUES
               (
                default, @projName, @description, @ownerEmail, @azureServices, 
                @clarityId, @eHlccd, @costCenter, @billingContact, @createddate, 
                @modifieddate,@createdby, @modifiedby
               );";
        }

        public static void ExecuteTSqlNonQuery(
            string projName, string description, string ownerEmail, string azureServices, string clarityId, 
            string eHlccd, string costCenter, string billingContact, DateTime createddate, DateTime modifieddate, string createdby,
            string modifiedby)
        {
            try
            {
                
                var conString = CloudNinjaDBConString;
                using SqlConnection con = new SqlConnection();
                con.ConnectionString = conString;
                con.AccessToken = GetTokenForCloudNinjaDb();
                con.Open();
                string sqlCommandText = InsertIntoProjectInfoTable();
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                using SqlCommand command = new SqlCommand(sqlCommandText, con);
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                command.Parameters.AddWithValue("@projName", projName);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@ownerEmail", ownerEmail);
                command.Parameters.AddWithValue("@azureServices", azureServices);
                command.Parameters.AddWithValue("@clarityId", clarityId);
                command.Parameters.AddWithValue("@eHlccd", eHlccd);
                command.Parameters.AddWithValue("@costCenter", costCenter);
                command.Parameters.AddWithValue("@billingContact", billingContact);
                command.Parameters.AddWithValue("@createddate", createddate);
                command.Parameters.AddWithValue("@modifieddate", modifieddate);
                command.Parameters.AddWithValue("@createdby", createdby);
                command.Parameters.AddWithValue("@modifiedby", modifiedby);

               
                int rowsAffected = command.ExecuteNonQuery();
                Console.Out.WriteLine($"\t {rowsAffected} rows affected");
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine($"{ex.Message}");
               
            }
        }

        //private static IList<string> QueryProjectInfoTable()
        //{
        //    IList<string> queryResult = new List<string>();
        //    var conString = CloudNinjaDBConString;
        //    using SqlConnection con = new SqlConnection();
        //    con.ConnectionString = conString;
        //    con.AccessToken = GetTokenForCloudNinjaDb();
        //    con.Open();
        //    var sqlStatement = "Select * from dbo.ProjectInfo";
        //    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
        //    {
        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                //Console.WriteLine($"Row: {reader.GetValue(1)}");
        //                var rowData = $"GUID: {reader.GetValue(0)}, ProjectName: {reader.GetValue(1)}, Description: {reader.GetValue(2)}";
        //                queryResult.Add(rowData);
        //            }
        //        }
        //    }
        //    return queryResult;
        //}
    }
}
