using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Agile.Library.Teams;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureWebApp.Function
{
    public static class TeamsApi
    {
        [FunctionName("GetTeams")]
        public static async Task<IActionResult> GetTeams([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Return a list of teams");
            var teams = await Teams.GetTeamsAsync();
            return new OkObjectResult(teams);
        }

        [FunctionName("GetTeam")]
        public static async Task<IActionResult> GetEmployee([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var id = req.Query["teamId"];
            log.LogInformation("Return a team");
            var team = await Teams.GetTeamAsync(int.Parse(id));
            return new OkObjectResult(team);
        }
    }
}
