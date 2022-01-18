using Agile.Library.Teams.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Agile.Library.Teams
{
    public static class Employees    
    {
        public static async Task<List<Employee>> GetEmployeesAsync()
        {
            var employee = new List<Employee>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT Id, Email,  Firstname, Lastname, Notes, InDevops, InTeams FROM Employees");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    employee.Add(new Employee { Id = (int)row["Id"], Firstname= (string)row["Firstname"], Lastname = (string)row["Lastname"], Email = (string)row["Email"], 
                        Notes = Convert.IsDBNull(row["Notes"]) ? "" : (string)(row["Notes"]??""),
                        InDevops = Convert.IsDBNull(row["InDevops"]) ? false : (bool)(row["InDevops"] ?? ""),
                        InTeaams = Convert.IsDBNull(row["InTeams"]) ? false : (bool)(row["InTeams"] ?? "")
                    });
                }
            }
            return employee;
        }

        public static async Task<List<Employee>> GetEmployeesAsync(Team team)
        {
            var employee = new List<Employee>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT EmployeeId, EmployeeEmail,  EmployeeFirstname, EmployeeLastname, EmployeeNotes, EmployeeInDevops, EmployeeInTeams, RoleId, RoleName, TeamId, TeamName FROM ViewEmployees TeamId={team.Id}");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    employee.Add(new Employee {
                        Id = (int)row["EmployeeId"], Firstname = (string)row["EmployeeFirstname"], Lastname = (string)row["EmployeeLastname"], Email = (string)row["EmployeeEmail"], 
                        Notes = Convert.IsDBNull(row["EmployeeNotes"]) ? "" : (string)(row["EmployeeNotes"] ?? ""),
                        InDevops = Convert.IsDBNull(row["EmployeeInDevops"]) ? false : (bool)(row["EmployeeInDevops"] ?? ""),
                        InTeaams = Convert.IsDBNull(row["EmployeeInTeams"]) ? false : (bool)(row["EmployeeInTeams"] ?? "")
                    });
                }
            }
            return employee;
        }
        public static async Task<List<Employee>> GetEmployeesAsync(Role role)
        {
            var employee = new List<Employee>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT EmployeeId, Firstname FROM ViewEmployees WHERE RoleId={role.Id}");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    employee.Add(new Employee { Id = (int)row["EmployeeId"], Firstname = (string)row["Firstname"] });
                }
            }
            return employee;
        }
        public static async Task<Employee> GetEmployeeAsync(int Id)
        {
            Employee employee = null;
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT Id, Email,  Firstname, Lastname, Notes, InDevops, InTeams FROM Employees WHERE Id ='{Id}'");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    employee = new Employee
                    {
                        Id = (int)row["Id"],
                        Firstname = (string)row["Firstname"],
                        Lastname = (string)row["Lastname"],
                        Email = (string)row["Email"],
                        Notes = Convert.IsDBNull(row["Notes"]) ? "" : (string)(row["Notes"] ?? ""),
                        InDevops = Convert.IsDBNull(row["InDevops"]) ? false : (bool)(row["InDevops"] ?? ""),
                        InTeaams = Convert.IsDBNull(row["InTeams"]) ? false : (bool)(row["InTeams"] ?? "")
                    };
                }
            }
            if (employee == null) throw new Exception($"Employee with id {Id} not found");
            return employee;
        }

        public static async Task CreateEmployeeAsync(Employee employee)
        {
            employee.Id = await Database.ExecuteScalarCommandAsync($"INSERT INTO Persons (Name) OUTPUT INSERTED.ID VALUES('{employee.Firstname}')");
        }
        public static async void UpdatePersonAsync(Employee employee)
        {
            await Database.ExecuteCommandAsync($"UPDATE Persons SET Name='{employee.Firstname}' WHERE Id='{employee.Id}'");
        }
    }
}
