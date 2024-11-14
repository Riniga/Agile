using Agile.Library;
using Agile.Library.Teams;
using System;
using System.Diagnostics;
using System.IO;

namespace Agile.Command
{
    class Program
    {   
        static void Main(string[] args)
        {
            ConfigSettings();
            if (args.Length > 0 && args[0] == "projects") DisplayProjects();
            else
            {
                ExportTeams("teams.txt");
                Process.Start("notepad.exe", "Teams.txt");
            }
        }

        private static void ConfigSettings()
        {
            Settings.UseCache = false; // You must start "Cosmos DB Emulator"
            Settings.DevopsPersonalaccesstoken = Environment.GetEnvironmentVariable("AZURE_DEVOPS_PAT"); 

            Settings.DevopsOrganization = "skanskanordic";
            Settings.DevopsProject = "0439fbd7-edf7-4560-81a5-d10eb74f33d3"; //Skanska Sverige IT
            //Settings.project = "28de8608-aa74-4d08-bdbe-1321f5d033f7"; //HQ-Infrastructure

            Settings.CosmosDbEndpointUrl = "https://localhost:8081";
            Settings.CosmosDbPrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            Settings.CosmosDbDatabaseId = "Agile";
        }

        private static void DisplayProjects()
        {
            var projects = Projects.Instance.All;
            foreach (var project in projects)
            {
                System.Console.WriteLine("--- " + project.name + "("+project.id+")");
            }
        }

        private static void ExportTeams(string filename)
        {

            var teams = Teams.Instance.All;
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (var team in teams)
                {
                    foreach (var member in team.Members)
                    {
                        writer.WriteLine(member.uniqueName + "|" + member.displayName +  "|" + team.name);
                    }
                }
            }
        }
    }
}
