using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Agile.Library.Teams;
using System.Linq;
using System.Collections.Generic;
using Agile.Library.Teams.Model;

namespace AzureWebApp.Function
{
    public static class RolesApi
    {
        [FunctionName("GetRoles")]
        public static async Task<IActionResult> GetRoles([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Return a list of roles");
            List<Role> roles = new List<Role>();

            var membersWithroles = Roles.Instance.All;
            foreach (var row in membersWithroles)
            {
                if (roles.Where(role=>role.Id == row.Role.Id).Count()==0) roles.Add(row.Role);
            }
            
            return new OkObjectResult(roles);
        }

        [FunctionName("GetRole")]
        public static async Task<IActionResult> GetRole([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var id = req.Query["roleId"];
            log.LogInformation("Featch and return role with id: " + id);
            return new OkObjectResult(Roles.Instance.All.Where(role => role.Role.Id == int.Parse(id)).FirstOrDefault());
        }

        [FunctionName("GetEmployeesWithRole")]
        public static async Task<IActionResult> GetEmployeesWithRole([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var id = int.Parse(req.Query["roleId"]);
            log.LogInformation("Return all employee with role with id: " + id);
            var employeesInTeam = Employees.Instance.All.Where(e => e.RoleInTeam.Where(r => r.Role.Id == id).Count() > 0);
            return new OkObjectResult(employeesInTeam);
        }
    }
}
