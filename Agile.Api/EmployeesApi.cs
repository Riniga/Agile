using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Agile.Library.Teams;

namespace Agile.Api
{
    public static class EmployeesApi
    {
        [FunctionName("GetEmployees")]
        public static async Task<IActionResult> GetEmployees([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Return a list of employees");
            return new OkObjectResult(Employees.Instance.All);
        }

        [FunctionName("GetEmployee")]
        public static async Task<IActionResult> GetEmployee([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var id = req.Query["employeeId"];
            log.LogInformation("Return an employee");
            var employee = Employees.Instance.GetEmployee(id);
            return new OkObjectResult(employee);
        }
    }
}
