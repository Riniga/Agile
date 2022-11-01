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
        public static Projects Instance { get { return lazy.Value; } }

        public List<Project> All;
        private Projects()
        {
            All = GetProjectsAsync().Result;
        }
        private async Task<List<Project>> GetProjectsAsync()
        {
            var cacheFile = "projects.json";
            var projects = new List<Project>();

            if (File.Exists(cacheFile) && File.GetCreationTime(cacheFile) >DateTime.Now.AddMinutes(-60) )
            {
                projects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Project>>(File.ReadAllText(cacheFile));
            }
            else
            { 
                try
                {
                    var organization = "skanskanordic";
                    var project = "0439fbd7-edf7-4560-81a5-d10eb74f33d3";
                    var personalaccesstoken = "6yw2enihff7pvygx5kacfnhqovub4yfbmlsdlube3mdub2qmrasa";
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalaccesstoken))));
                        using (HttpResponseMessage response = client.GetAsync($"https://dev.azure.com/{organization}/_apis/projects").Result)
                        {
                            response.EnsureSuccessStatusCode();
                            string responseBody = await response.Content.ReadAsStringAsync();
                            var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectsResponse>(responseBody);
                            foreach (var currentproject in responseObject.value)
                            {
                                //currentproject.Admins = GetProjectAdminsAsync(currentproject.id).Result;
                                projects.Add(currentproject);
                            }
                            
                            Console.WriteLine("Done! Retrieved all teams with team members!");
                        }
                    }
                }
                catch (Exception ex){ Console.WriteLine("boom: " + ex.Message); }
                File.Delete(cacheFile);
                File.WriteAllText(cacheFile, Newtonsoft.Json.JsonConvert.SerializeObject(projects));
            }
            return projects;
        }
        private async Task<List<Employee>> GetProjectAdminsAsync(string projectId)
        {
            var admins = new List<Employee>();

            try
            {
                var organization = "skanskanordic";
                var project = "0439fbd7-edf7-4560-81a5-d10eb74f33d3";
                var personalaccesstoken = "6yw2enihff7pvygx5kacfnhqovub4yfbmlsdlube3mdub2qmrasa";
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
