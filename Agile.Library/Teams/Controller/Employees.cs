using Agile.Library.Teams.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Agile.Library.Teams
{
    public sealed class Employees    
    {
        private static readonly Lazy<Employees> lazy = new Lazy<Employees>(() => new Employees());
        public static Employees Instance { get { return lazy.Value; } }

        public List<Employee> All;
        private Employees()
        {
            //var task = Task.Run(async () => await GetEmployeesAsync());
            All = GetEmployeesAsync().Result;// task.Result;
        }
        private async Task<List<Employee>> GetEmployeesAsync()
        {
            var employees = new List<Employee>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT EmployeeId, EmployeeEmail,  EmployeeFirstname, EmployeeLastname, EmployeeNotes, EmployeeInDevops, EmployeeInTeams, RoleId, RoleName, TeamId, TeamName FROM ViewEmployees");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    var employee = employees.Where(e => e.Id == (int)row["EmployeeId"]).FirstOrDefault();
                    var role = Convert.IsDBNull(row["RoleId"]) ? null : Roles.Instance.All.Where(role => role.Id == (int)row["RoleId"]).FirstOrDefault();
                    var team = Convert.IsDBNull(row["TeamId"]) ? null : Teams.Instance.All.Where(team => team.Id == (int)row["TeamId"]).FirstOrDefault();

                    if (employee == null)
                    { 
                        employee = new Employee
                        {
                            Id = (int)row["EmployeeId"],
                            Firstname = (string)row["EmployeeFirstname"],
                            Lastname = (string)row["EmployeeLastname"],
                            Email = (string)row["EmployeeEmail"],
                            Notes = Convert.IsDBNull(row["EmployeeNotes"]) ? "" : (string)(row["EmployeeNotes"] ?? ""),
                            InDevops = Convert.IsDBNull(row["EmployeeInDevops"]) ? false : (bool)(row["EmployeeInDevops"] ?? ""),
                            InTeams = Convert.IsDBNull(row["EmployeeInTeams"]) ? false : (bool)(row["EmployeeInTeams"] ?? ""),
                            RoleInTeam = new List<RoleInTeam>() 
                        };
                        if (role != null && team != null) employee.RoleInTeam.Add(new RoleInTeam() { Role = role, Team = team });
                        employees.Add(employee);
                    }
                    else
                    {
                        if (role != null && team != null) employee.RoleInTeam.Add(new RoleInTeam() { Role = role, Team = team });
                    }
                }
            }
            return employees;
        }

        public void AddTeamRoleForEmployee(Employee employee, Team team, Role role)
        {
            var data = Database.GetDataSetAsync("SELECT EmployeeId FROM [RoleInTeam] WHERE [EmployeeId]='" + employee.Id + "' AND [TeamId]='" + team.Id+ "' AND [RoleId]='" + role.Id + "' ").Result;
            if (data.Tables[0].Rows.Count > 0) return;

            var task= Task.Run(async () => await Database.ExecuteCommandAsync("INSERT INTO [RoleInTeam] ([EmployeeId],[TeamId],[RoleId]) VALUES ('" + employee.Id + "','" + team.Id + "','" + role.Id + "') "));
            var id = task.Result;
            if (id!=0) employee.RoleInTeam.Add(new RoleInTeam() { Role = role, Team = team });
        }

        public void AddEmployee(Employee employee)
        {
            if (EmployeeExistInList(employee)) return;
            if (EmployeeExistInDatabase(employee)){ All.Add(employee); return; }
            var task = Task.Run(async () => await Database.ExecuteCommandAsync("INSERT INTO [Employees] ([Email],[Firstname],[Lastname]) VALUES ('" + employee.Email+ "','" + employee.Firstname + "','" + employee.Lastname + "') "));
            var id = task.Result;
            if (id != 0) All.Add(employee);
        }

        private bool EmployeeExistInDatabase(Employee employee)
        {
            var data = Database.GetDataSetAsync("SELECT Id FROM [Employees] WHERE [Email]='" + employee.Email + "'").Result;
            return (data.Tables[0].Rows.Count > 0);
        }

        private bool EmployeeExistInList(Employee employee)
        {
            return All.Where(currentEmployee => currentEmployee.Email == employee.Email).Count() > 0;
        }

        public static async Task<List<Employee>> GetEmployeesAsync(Team team)
        {
            
            var employee = new List<Employee>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT EmployeeId, EmployeeEmail,  EmployeeFirstname, EmployeeLastname, EmployeeNotes, EmployeeInDevops, EmployeeInTeams, RoleId, RoleName, TeamId, TeamName FROM ViewEmployees WHERE TeamId={team.Id}");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    employee.Add(new Employee {
                        Id = (int)row["EmployeeId"], Firstname = (string)row["EmployeeFirstname"], Lastname = (string)row["EmployeeLastname"], Email = (string)row["EmployeeEmail"], 
                        Notes = Convert.IsDBNull(row["EmployeeNotes"]) ? "" : (string)(row["EmployeeNotes"] ?? ""),
                        InDevops = Convert.IsDBNull(row["EmployeeInDevops"]) ? false : (bool)(row["EmployeeInDevops"] ?? ""),
                        InTeams = Convert.IsDBNull(row["EmployeeInTeams"]) ? false : (bool)(row["EmployeeInTeams"] ?? "")
                    });
                }
            }
            return employee;
        }
        public static async Task<List<Employee>> GetEmployeesAsync(Role role)
        {
            var employees = new List<Employee>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT EmployeeId, EmployeeEmail, EmployeeFirstname, EmployeeLastname FROM ViewEmployees WHERE RoleId={role.Id}");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    employees.Add(new Employee { Id = (int)row["EmployeeId"], Email = (string)row["EmployeeEmail"], Firstname = (string)row["EmployeeFirstname"], Lastname= (string)row["EmployeeLastname"] });
                }
            }
            return employees;
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
                        InTeams = Convert.IsDBNull(row["InTeams"]) ? false : (bool)(row["InTeams"] ?? "")
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
