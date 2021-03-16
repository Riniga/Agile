using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AzureFunction.Business;
using AzureFunction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunction
{
    public static class Functions
    {
        [FunctionName("SaveUserHealthRadarResult")]
        public static async Task<IActionResult> SaveUserHealthRadarResult([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,ILogger log)
        {
            log.LogInformation("Save Health Radar Result: " + req.Body);
            
            string requestBody = String.Empty;
            using (StreamReader streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }
            log.LogInformation(requestBody);
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var result = new HealthRadarResult();
            string tempId = data.id;

            if (!string.IsNullOrEmpty(tempId)) result.Id = new Guid(tempId);
            result.Area = data.area;
            result.Role = data.role;
            result.Answers = data.answers.ToObject<List<int>>();
            result.Posted = DateTime.Now;

            if (result.Id==Guid.Empty)
            {
                log.LogInformation("Create new record");
                await CosmosHelper.CreateResultInCosmosDb(result);
                
            }
            else
            {
                try
                {
                    log.LogInformation("Update record: " + result.Id);
                    await CosmosHelper.UpdateResultInCosmosDb(result);
                }catch(Exception e)
                {
                    log.LogInformation("Create new record");
                    await CosmosHelper.CreateResultInCosmosDb(result);
                }
            }

            return new OkObjectResult(result.Id);
        }
        [FunctionName("GetAllHealthRadarResults")]
        public static async Task<IActionResult> GetAllHealthRadarResults([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Return a list of all users health radar results");
            var results = await CosmosHelper.GetAllResultsFromCosmosDb();
            return new OkObjectResult(results);
        }

    }
}
