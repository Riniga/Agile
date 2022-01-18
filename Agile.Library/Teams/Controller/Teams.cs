using Agile.Library.Teams.Enum;
using Agile.Library.Teams.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Agile.Library.Teams
{
    public static class Teams
    {
        public static async Task<List<Team>> GetTeamsAsync()
        {
            var teams = new List<Team>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT TeamId, Teamname, TeamType FROM ViewTeams");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    teams.Add(new Team 
                    { 
                        Id = (int)row["TeamId"], 
                        Name = (string)row["TeamName"], 
                        TeamType= (TeamTypes)System.Enum.Parse(typeof(TeamTypes), (string)row["TeamType"]),
                    });
                }
            }
            return teams;
        }
        public static async Task<List<Team>> GetTeamsAsync(Employee employee)
        {
            var employeeTeams= new List<Team>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT TeamId, TeamName, TeamType FROM ViewEmployeeTeams WHERE EmployeeId='{employee.Id}'");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    employeeTeams.Add(new Team { Id = (int)row["TeamId"], Name = (string)row["TeamName"], TeamType = (TeamTypes)System.Enum.Parse(typeof(TeamTypes), (string)row["TeamType"]) });
                }
            }
            return employeeTeams;
        }

        public static async Task<Team> GetTeamAsync(int Id)
        {
            Team team = null;
            var contracts = new List<Team>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT Id, Name FROM Teams WHERE Id ='{Id}'");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    team = new Team { Id = (int)row["Id"], Name = (string)row["Name"] };
                }
            }
            if (team == null) throw new Exception($"Team with id {Id} not found");
            return team;
        }
        public  static async Task<int> CreateTeamtAsync(Team team)
        {
            return await Database.ExecuteCommandAsync($"INSERT INTO Teams (Name, TeamTypeId) OUTPUT INSERTED.ID VALUES('{team.Name}', '{(int)team.TeamType}')");
        }
    }
}
