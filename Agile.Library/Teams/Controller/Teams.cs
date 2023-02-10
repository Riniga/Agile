using Agile.Library.Teams.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Agile.Library.Teams
{
    public sealed class Teams
    {
        public static readonly Lazy<Teams> lazy = new Lazy<Teams>(() => new Teams());
        public static string cacheKey = "teams_" + Settings.DevopsProject;

        public static Teams Instance { get { return lazy.Value; } }

        public List<Team> All;
        private Teams()
        {

            All = GetTeamsAsync().Result;
        }
        private async Task<List<Team>> GetTeamsAsync()
        {

            var teams = new List<Team>();
            if (Settings.UseCache)
            { 
                teams = await CosmosCache<List<Team>>.Get(cacheKey);
                if (teams!=null && teams.Count>0) return teams;
            }

            try
            {
                teams = new List<Team>();
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Settings.DevopsPersonalaccesstoken))));
                    using (HttpResponseMessage response = client.GetAsync($"https://dev.azure.com/{Settings.DevopsOrganization}/_apis/projects/{Settings.DevopsProject}/teams").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TeamsResponse>(responseBody);
                        foreach (var team in responseObject.value)
                        {
                            team.Members = GetTeamMembersAsync(team.id).Result;
                            teams.Add(team);
                        }
                        Console.WriteLine("Done! Retrieved all teams with team members!");
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("boom: " + ex.Message); }
            if (Settings.UseCache) await CosmosCache<List<Team>>.Set(cacheKey, teams, 60);
            return teams;
        }
        private async Task<List<Employee>> GetTeamMembersAsync(string teamId)
        {
            var members = new List<Employee>();

            try
            {
                
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Settings.DevopsPersonalaccesstoken))));
                    using (HttpResponseMessage response = client.GetAsync($"https://dev.azure.com/{Settings.DevopsOrganization}/_apis/projects/{Settings.DevopsProject}/teams/{teamId}/members?api-version=6.0").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TeamMembersResponse>(responseBody);
                        foreach (var member in responseObject.value)
                        {
                            //var roleinteam = Roles.Instance.All.Where(memberroleinteam => memberroleinteam.MemberId == member.identity.Id && memberroleinteam.TeamId==teamId);
                            //member.identity.RoleInTeam = new List<RoleInTeam>();
                            //foreach (var role in roleinteam)
                            //{
                            //    member.identity.RoleInTeam.Add(new RoleInTeam { Role= role.Role, TeamId = teamId  }) ;
                            //}
                            members.Add(member.identity);
                        }
                        Console.WriteLine("Done! Retrieved team members for team: " + teamId);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("boom: " + ex.Message); }
            return members;
        }
        private class TeamsResponse { public List<Team> value; }
        private class TeamMembersResponse { public List<Member> value; }
        private class Member { public Employee identity; }
    }
}
