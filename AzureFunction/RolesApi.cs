using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Agile.Library.Teams;
using System.Linq;
using System.Collections.Generic;
using System;

namespace AzureWebApp.Function
{
    public static class RolesApi
    {
        [FunctionName("GetRoles")]
        public static async Task<IActionResult> GetTeams([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Return a list of roles");
            return new OkObjectResult(Roles.Instance.All);
        }

        [FunctionName("GetRole")]
        public static async Task<IActionResult> GetRole([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var id = req.Query["roleId"];
            log.LogInformation("Featch and return role with id: " + id);
            return new OkObjectResult(Roles.Instance.All.Where(role => role.Id == int.Parse(id)).FirstOrDefault());
        }

        [FunctionName("GetEmployeesWithRole")]
        public static async Task<IActionResult> GetEmployeesWithRole([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var id = int.Parse(req.Query["roleId"]);
            log.LogInformation("Return all employee with role with id: " + id);

            var result = new List<EmployeeWithRole>();
            var employeesInTeam = Employees.Instance.All.Where(e => e.RoleInTeam.Where(r => r.Role.Id == id).Count() > 0);
            foreach (var employee in employeesInTeam)
            {
                var current = new EmployeeWithRole();
                var roleAssignmentStrings = employee.RoleInTeam.Where(r => r.Role.Id == id).Select(r => r.Team.Name).ToList();
                current.name = employee.Firstname + " " + employee.Lastname;
                current.team = String.Join(',', roleAssignmentStrings);
                current.id = employee.Id;
                result.Add(current);
            }
            return new OkObjectResult(result);

        }
        class EmployeeWithRole
        {
            public int id;
            public string name;
            public string team;
        }

    }
}
