using Agile.Library.Teams;
using Agile.Library.Teams.Enum;
using Agile.Library.Teams.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Agile.Console
{
    class Program
    {   
        static void Main(string[] args)
        {
            //Seed.SeedAll();
            //DisplayTeam("ITLT");
            DisplayTeams();
        }

        private static void DisplayTeam(string teamName)
        {
            var team = Teams.Instance.All.Where(currentTeam => currentTeam.Name == teamName).FirstOrDefault();
            
            System.Console.WriteLine("+" + team.Name);
            var employeesInTeam = Employees.Instance.All.Where(e => e.RoleInTeam.Where(r => r.Team.Id == team.Id).Count() > 0);
            foreach (var employee in employeesInTeam)
            {
                var roleAssignmentStrings = employee.RoleInTeam.Where(r => r.Team.Id == team.Id).Select(r => r.Role.Name).ToList();
                System.Console.WriteLine("  -" + employee.Firstname + " " + employee.Lastname + "(" + String.Join(',', roleAssignmentStrings) + ")");
            }
            
        }
        private static void DisplayTeams()
        {
            var teams = Teams.Instance.All;
            foreach (var team in teams)
            {
                DisplayTeam(team.Name);
            }
        }

        private static void DisplayRoles()
        {
            var roles = Roles.Instance.All;
            foreach (var role in roles)
            {
                System.Console.WriteLine("+" + role.Name);
                var employeesWithrole = Employees.Instance.All.Where(e => e.RoleInTeam.Where(r => r.Role.Id == role.Id).Count() > 0);
                foreach (var employee in employeesWithrole)
                {
                    var roleAssignmentStrings = employee.RoleInTeam.Where(r => r.Role.Id == role.Id).Select(r=>r.Team.Name).ToList();
                    System.Console.WriteLine("  -" + employee.Firstname + " " + employee.Lastname + "(" + String.Join(',' ,roleAssignmentStrings)+ ")");
                }
            }
        }
    }
}
