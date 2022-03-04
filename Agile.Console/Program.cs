using Agile.Library.Teams;
using Agile.Library.Teams.Enum;
using Agile.Library.Teams.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Agile.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var teams = GetAllTeam();
            foreach (var team in teams)
                System.Console.WriteLine(team.Name);
            var roles = GetAllRoles();


            // Import all roles
        }

        private static List<Team> GetAllTeam()
        {
            return Teams.GetTeamsAsync().Result;
        }

        private static List<Role> GetAllRoles()
        {
            return Roles.GetRolesAsync().Result;
        }
    }
}
