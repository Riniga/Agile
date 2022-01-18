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
            var teams = CreateAllTeam();
            
            //var roles = CreateAllRoles();

            // Import all roles
        }

        private static List<Team> CreateAllTeam()
        {
            var teamsFileData = File.ReadAllText("teams.txt");
            var rows = teamsFileData.Split("\r\n");
            foreach (var row in rows)
            {

                var cols = row.Split(";");
                var team = new Team
                {
                    Name = (string)cols[1],
                    TeamType = (TeamTypes)System.Enum.Parse(typeof(TeamTypes), (string)cols[0])
                };

                team.Id = Teams.CreateTeamtAsync(team).Result;


            }
            return Teams.GetTeamsAsync().Result;
        }

        private static List<Role> CreateAllRoles()
        {
            var teamsFileData = File.ReadAllText("roles.txt");
            var rows = teamsFileData.Split("\r\n");
            foreach (var row in rows)
            {

                var cols = row.Split(";");
                var role = new Role
                {
                    Name = (string)cols[1],
                    RoleType = (RoleTypes)System.Enum.Parse(typeof(RoleTypes), (string)cols[0])
                };

                role.Id = Roles.CreateRoleAsync(role).Result;


            }
            return Roles.GetRolesAsync().Result;
        }

    }
}
