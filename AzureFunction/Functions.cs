using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System.Collections.Generic;

namespace AzureFunction
{
    public static class Functions
    {
        [FunctionName("SaveUserHealtRadarResult")]
        public static async Task<IActionResult> SaveUserHealtRadarResult([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,ILogger log)
        {
            log.LogInformation("Save Health Radar Result for user. body=: " + req.Body);
            return new OkObjectResult("Okey");
        }
    }
}
