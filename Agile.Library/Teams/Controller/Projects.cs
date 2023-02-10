using Agile.Library.Teams.Enum;
using Agile.Library.Teams.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


//https://dev.azure.com/skanskanordic/_apis/projects?api-version=6.0
//https://dev.azure.com/skanskanordic/_apis/asdfasdfasdf/properties?api-version=6.0-preview.1

namespace Agile.Library.Teams
{
    public sealed class Projects
    {
        private static readonly Lazy<Projects> lazy = new Lazy<Projects>(() => new Projects());
        //private static string personalaccesstoken = "r73bcrgl5xeeuhlgeo7qx6w57wu2sw7rwqv5s32qvbidm3rzv7na"; // Skanska Agie
        private static string personalaccesstoken = "64qnswlxedpllzovjq5kzwymf2n5it743hwzrpmrhujvqllljeva"; // Skanska Agie
        private static string organization = "skanskanordic";
        private static string project = "0439fbd7-edf7-4560-81a5-d10eb74f33d3";
        private static string cacheKey = "projects_" + project;

        public static Projects Instance { get { return lazy.Value; } }

        public List<Project> All;
        private Projects()
        {
            All = GetProjectsAsync().Result;
        }
        private async Task<List<Project>> GetProjectsAsync()
        {
            var projects = await CosmosCache<List<Project>>.Get(cacheKey);
            if (projects != null && projects.Count > 0) return projects;
            try
            {
                
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalaccesstoken))));
                    using (HttpResponseMessage response = client.GetAsync($"https://dev.azure.com/{organization}/_apis/projects?$top=1000").Result)
                    {
                        projects = new List<Project>();
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectsResponse>(responseBody);
                        foreach (var currentproject in responseObject.value)
                        {
                            //currentproject.Admins = GetProjectAdminsAsync(currentproject.id).Result;
                            projects.Add(currentproject);
                        }
                            
                        Console.WriteLine("Done! Retrieved all teams!");
                    }
                }
            }
            catch (Exception ex){ Console.WriteLine("boom: " + ex.Message); }
            if (projects!= null) await CosmosCache<List<Project>>.Set(cacheKey, projects, 60);
            return projects;
        }
        private async Task<List<Employee>> GetProjectAdminsAsync(string projectId)
        {
            var admins = new List<Employee>();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalaccesstoken))));
                    using (HttpResponseMessage response = client.GetAsync($"https://dev.azure.com/{organization}/_apis/projects/{projectId}/admins?api-version=6.0").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TeamMembersResponse>(responseBody);
                        foreach (var member in responseObject.value)
                        {
                            admins.Add(member.identity);
                        }
                        Console.WriteLine("Done! Retrieved team members for team: " + projectId);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("boom: " + ex.Message); }
            return admins;
        }
        private class ProjectsResponse { public List<Project> value; }
        private class TeamMembersResponse { public List<Member> value; }
        private class Member { public Employee identity; }
    }
}
