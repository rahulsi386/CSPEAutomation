using System;
using System.Collections.Generic;
using System.Text;
using Azure.Identity;
using Azure.Core;
using System.Data.SqlClient;

namespace CloudNinjaFunctions.Helpers
{
    public class CloudNinjaDBOps
    {
        //Database connection string
        private const string _cloudNinjaDBConnectionString = "CloudNinjaDB_ConString";
        private static readonly string CloudNinjaDBConString = Environment.GetEnvironmentVariable(_cloudNinjaDBConnectionString);

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

        public static string InsertIntoProjectStatusTable()
        {
            return @"
               INSERT INTO dbo.ProjectStatus 
               (
                proj_id, proj_request_rcvd,proj_assigned_to,initial_meeting,
                get_requirements,crt_visio,crt_sizingconfig_doc,updt_sdd,rvw_sdd_prjtm,
                rvw_sdd_tad,rvw_sdd_isrm,cspe_signed,appmgr_signed,tad_signed,isrm_signed,sdm_signed,
                crt_deploydoc,deploydoc_approval,aad_approval_appreg,crt_appreg,azres_deployment,
                nwfw_team_approval,apply_nwfw_config,iam_team_approval,grant_roles_perms,crt_handover_doc,send_handover_doc,
                created_date,modified_date,created_by, modified_by
               ) 
               VALUES
               (
                @projid, 'Yes', 'Pending','Pending','Pending','Pending','Pending','Pending','Pending','Pending','Pending',
                'Pending','Pending','Pending','Pending','Pending','Pending','Pending','Pending','Pending','Pending','Pending',
                'Pending','Pending','Pending','Pending','Pending',
                @createddate, @modifieddate,@createdby, @modifiedby
               );";
        }
        public static void ExecuteInsertIntoProjectInfoTable(
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

        public static void ExecuteInsertIntoProjectStatusTable(
        string projid, DateTime createddate, DateTime modifieddate, string createdby, string modifiedby)
        {
            try
            {

                var conString = CloudNinjaDBConString;
                using SqlConnection con = new SqlConnection();
                con.ConnectionString = conString;
                con.AccessToken = GetTokenForCloudNinjaDb();
                con.Open();
                string sqlCommandText = InsertIntoProjectStatusTable();
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                using SqlCommand command = new SqlCommand(sqlCommandText, con);
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                command.Parameters.AddWithValue("@projid", projid);
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

        private static IList<string> QueryProjectInfoTable()
        {
            IList<string> queryResult = new List<string>();
            var conString = CloudNinjaDBConString;
            using SqlConnection con = new SqlConnection();
            con.ConnectionString = conString;
            con.AccessToken = GetTokenForCloudNinjaDb();
            con.Open();
            var sqlStatement = "Select * from dbo.ProjectInfo";
            using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Console.WriteLine($"Row: {reader.GetValue(1)}");
                        var rowData = $"GUID: {reader.GetValue(0)}, ProjectName: {reader.GetValue(1)}, Description: {reader.GetValue(2)}";
                        queryResult.Add(rowData);
                    }
                }
            }
            return queryResult;
        }
    }
}
