using Agile.Library.Teams;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Agile.Console
{
    class Program
    {   
        static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("EndpointUrl", "https://localhost:8081");
            Environment.SetEnvironmentVariable("PrimaryKey", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            Environment.SetEnvironmentVariable("DatabaseId", "Agile");
            DisplayProjects();
            //DisplayTeams();
            //ExportTeams("Teams.txt");
            //Process.Start("notepad.exe", "Teams.txt");
        }
        private static void DisplayTeams()
        {
            var teams = Teams.Instance.All;
            foreach (var team in teams)
            {
                
                System.Console.WriteLine("--- " + team.name + " ---");
                DisplayTeam(team.id);
                System.Console.WriteLine("".PadLeft(50, '-'));
            }
        }

        private static void DisplayTeam(string teamId)
        {
            var team = Teams.Instance.All.Where(currentTeam => currentTeam.id == teamId).FirstOrDefault();
            foreach (var member in team.Members)
            {
                System.Console.WriteLine(member.Id + "|" + member.displayName + "|" + member.uniqueName);
            }
        }

        private static void DisplayProjects()
        {
            var projects = Projects.Instance.All;
            foreach (var project in projects)
            {
                System.Console.WriteLine("--- " + project.name + " ---");
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
