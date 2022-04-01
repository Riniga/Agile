using Agile.Library.Teams;
using Agile.Library.Teams.Enum;
using Agile.Library.Teams.Model;
using System;
using System.Linq;

namespace Agile.Console
{
    internal class Seed
    {
        public static void SeedAll()
        {
            //TODO: Issues with threadding, seed of roles and team must complete before seed of role assignments
            SeedRoles();
            SeedTeams();
            SeedEmployees();
            SeedTeamRoleForEmployees();
        }

        public static void SeedEmployees()
        {
            var employees = System.IO.File.ReadAllLines("Employees.txt");
            foreach (var employee in employees)
            {
                var parts = employee.Split('|');

                AddEmployee(parts[0], parts[1], parts[2]);
            }
        }

        

        public static void SeedTeams()
        {
            var teams = System.IO.File.ReadAllLines("Teams.txt");
            foreach (var team in teams)
            {
                var parts = team.Split('|');
                AddTeam(parts[0],  (TeamTypes)int.Parse(parts[1]) );
            }
        }

        
        public static void SeedRoles()
        {
            var roles = System.IO.File.ReadAllLines("Roles.txt");
            foreach (var role in roles)
            {
                AddRole(role);
            }
        }

        public static void SeedTeamRoleForEmployees()
        {
            var assignments = System.IO.File.ReadAllLines("TeamRoleForEmployees.txt");
            foreach (var assignment in assignments)
            {
                var parts = assignment.Split('|');
                AddTeamRoleForEmployee(parts[0], parts[1], parts[2]);
            }
        }

        private static void AddTeamRoleForEmployee(string employeeEmail, string teamName, string roleName)
        {
            var employees = Employees.Instance.All;
            Employee employee = employees.Where(employee => employee.Email == employeeEmail).FirstOrDefault();
            Role role = Roles.Instance.All.Where(role => role.Name == roleName).FirstOrDefault();
            Team team = Teams.Instance.All.Where(team => team.Name == teamName).FirstOrDefault();

            if (employee == null)
            { 
                //Log
            }else Employees.Instance.AddTeamRoleForEmployee(employee, team, role);
            //"Marknad & Försäljning"
            //"Marknad & Försäljning​"
        }
        private static void AddRole(string roleName)
        {
            var role = new Role() { Name = roleName, RoleType = RoleTypes.Agile };
            Roles.Instance.AddRole(role);
        }
        private static void AddTeam(string teamName, TeamTypes teamType)
        {
            var team = new Team() { Name = teamName, TeamType= teamType};
            Teams.Instance.AddTeam(team);
        }
        private static void AddEmployee(string email, string firstName, string lastName)
        {
            var employee = new Employee() { Email = email, Firstname = firstName, Lastname = lastName, RoleInTeam=new System.Collections.Generic.List<RoleInTeam>() };
            Employees.Instance.AddEmployee(employee);
        }


    }
}
