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
using System.Linq;
using System.Collections.Generic;

namespace AzureWebApp.Function
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
            var employee = await Employees.GetEmployeeAsync(int.Parse(id));
            return new OkObjectResult(employee);
        }

        //[FunctionName("CreatePerson")]
        //public static async Task<IActionResult> CreatePerson([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        //{
        //    string personJson = await new StreamReader(req.Body).ReadToEndAsync();
        //    var personObject = JObject.Parse(personJson);

        //    Console.WriteLine("Subscription:" + personObject);

        //    Person person = new Person();
        //    person.Name = (string)personObject["personName"];
        //    Contract contract = await Contracts.GetContractAsync((int)personObject["contractId"]);
        //    Persons.CreatePersonAsync(person, contract, (int)personObject["amount"]);

        //    log.LogInformation("Create a new person");

        //    return new OkObjectResult(true);
        //}
        //[FunctionName("UpdatePerson")]
        //public static async Task<IActionResult> UpdatePerson([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        //{
        //    string personJson = await new StreamReader(req.Body).ReadToEndAsync();
        //    var personObject = JObject.Parse(personJson);

        //    if (string.IsNullOrEmpty((string)personObject["personId"]))
        //    {
        //        throw new Exception("Unable to update user without ID");
        //    }

        //    Person person = new Person();
        //    person.Name = (string)personObject["personName"];
        //    person.Id = int.Parse((string)personObject["personId"]);

        //    Persons.UpdatePersonAsync(person);
        //    log.LogInformation("Updated person with ID: " + person.Id);
        //    return new OkObjectResult(true);
        //}
    }
}
