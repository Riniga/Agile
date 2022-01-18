using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Agile.Library.Teams;

namespace AzureWebApp.Function
{
    public static class RolesApi
    {
        [FunctionName("GetRoles")]
        public static async Task<IActionResult> GetTeams([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Return a list of roles");
            var roles = await Roles.GetRolesAsync();
            return new OkObjectResult(roles);
        }

        [FunctionName("GetRole")]
        public static async Task<IActionResult> GetEmployee([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var id = req.Query["roleId"];
            log.LogInformation("Return a role");
            var role = await Roles.GetRoleAsync(int.Parse(id));
            return new OkObjectResult(role);
        }
    }
}
