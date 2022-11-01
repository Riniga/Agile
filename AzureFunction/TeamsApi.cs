using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Agile.Library.Teams;
using System;
using System.Linq;

namespace AzureWebApp.Function
{
    public static class TeamsApi
    {
        [FunctionName("GetTeams")]
        public static async Task<IActionResult> GetTeams([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Return a list of teams");
            return new OkObjectResult(Teams.Instance.All);
        }
        [FunctionName("GetTeam")]
        public static async Task<IActionResult> GetTeam([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var id = req.Query["teamId"];
            log.LogInformation("Return team with id: " + id);
            return new OkObjectResult(Teams.Instance.All.Where(team=>team.Id==id).FirstOrDefault());

        }

        [FunctionName("GetEmployeesInTeam")]
        public static async Task<IActionResult> GetEmployeesInTeam([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var id = req.Query["teamId"];
            log.LogInformation("Return all employee in team with id: " + id);
            var employeesInTeam = Employees.Instance.All.Where(e => e.RoleInTeam.Where(r => r.TeamId == id).Count() > 0);
            return new OkObjectResult(employeesInTeam);

        }
    }
}
