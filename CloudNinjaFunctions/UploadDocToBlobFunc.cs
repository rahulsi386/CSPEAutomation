using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using Microsoft.Azure.Storage.DataMovement;
using Azure;
using System.Net;

namespace CloudNinjaFunctions
{
    public static class UploadDocToBlobFunc
    {
        public static string connectionString = "DefaultEndpointsProtocol=https;AccountName=cloudninjastoragev2;AccountKey=FeEz5QwktEqN0SWKY8EE5KuXJtZXmtUxqTc6TH2Ttr7r1JsNS74NXo83IiVGQWU9+N+IaH3guVgM2E8Vf4p6TQ==;EndpointSuffix=core.windows.net";
        public static string containerName = "diagramandconfig";
  

        [FunctionName("UploadDocToBlobFunc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Function invoked to upload file in Blob container.");

                string filename = req.Query["filename"];
                string filepath = req.Query["filepath"];

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                filename = filename ?? data?.FileName;
                filepath = filepath ?? data?.FilePath;

                BlobContainerClient blobContainer = new BlobContainerClient(connectionString, containerName);
                BlobClient blobClient = blobContainer.GetBlobClient(filename);
                using FileStream uploadFileStream = File.OpenRead(filepath);
                var blobResponse = blobClient.Upload(uploadFileStream, true);  
                uploadFileStream.Close();
                log.LogInformation("Upload BLOB function executed successfully.");
                return new OkObjectResult($"Status: {HttpStatusCode.OK} - Successfully added to BLOB.");
                
            }
            catch(RequestFailedException ex)
            {
                log.LogInformation($"Error: {ex.Message} \nTrace: {ex.StackTrace}");
                return new OkObjectResult($"Status: {HttpStatusCode.BadRequest} - {ex.Message} - {ex.StackTrace}");
            }
        }

        //private static bool CreateBlobFolder(string projid, string projname)
        //{
        //    var blobFolder = $"{projid}-{projname}";
        //    bool folderExists = Directory.Exists(blobFolder);
        //    if (!folderExists)
        //        Directory.CreateDirectory(blobFolder);
        //    return folderExists;
        //}

        //private static string GenerateBlobName(string projid, string doctype)
        //{
        //    string blobName = $"{projid}-{doctype}";
        //    return blobName;
        //}

        //private static void UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType)
        //{
        //    try
        //    {
        //        BlobContainerClient blobContainer = new BlobContainerClient(connectionString, containerName);
        //        BlobClient blobClient = blobContainer.GetBlobClient("RPSTestdoc");
        //        blobClient.Upload(@"D:\SPFx\AzureTestCode\azureCliCommands\armtemplate.json");
   
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Out.WriteLine(ex.Message);
        //    }
        //}
    }
}
