using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunction
{
    public class UserHealtRadarResult
    {
        public string SessionId {get; set;}
        public string Role {get; set;}
        public string Area {get; set;}
        public int[][] Question {get; set;}
    }
}
