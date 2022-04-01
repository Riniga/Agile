using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Agile.Library.Teams;
using System;
using System.Linq;
using System.Collections.Generic;

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
            var id = int.Parse(req.Query["teamId"]);
            log.LogInformation("Return team with id: " + id);
            return new OkObjectResult(Teams.Instance.All.Where(team=>team.Id==id).FirstOrDefault());
        }

        [FunctionName("GetEmployeesInTeam")]
        public static async Task<IActionResult> GetEmployeesInTeam([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var id = int.Parse(req.Query["teamId"]);
            log.LogInformation("Return all employee in team with id: " + id);

            var result = new List<EmployeeforTeam>();
            var employeesInTeam = Employees.Instance.All.Where(e => e.RoleInTeam.Where(r => r.Team.Id == id).Count() > 0);
            foreach (var employee in employeesInTeam)
            {
                var current = new EmployeeforTeam();
                var roleAssignmentStrings = employee.RoleInTeam.Where(r => r.Team.Id == id).Select(r => r.Role.Name).ToList();
                current.name = employee.Firstname + " " + employee.Lastname;
                current.role = String.Join(',', roleAssignmentStrings);
                current.id = employee.Id;
                result.Add(current);
            }
            return new OkObjectResult(result);

        }
        class EmployeeforTeam
        {
            public int id;
            public string name;
            public string role;
        }

    }
}
