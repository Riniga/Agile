using Agile.Library.Teams;
using Agile.Library.Teams.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Agile.Console
{
    class Program
    {   
        static void Main(string[] args)
        {
            //DisplayTeams();
            ExportTeams("Teams.txt");
            Process.Start("notepad.exe", "Teams.txt");

        }
        private static void DisplayTeams()
        {
            var teams = Teams.Instance.All;
            foreach (var team in teams)
            {
                
                System.Console.WriteLine("--- " + team.Name + " ---");
                DisplayTeam(team.Id);
                System.Console.WriteLine("".PadLeft(50, '-'));
            }
        }

        private static void DisplayTeam(string teamId)
        {
            var team = Teams.Instance.All.Where(currentTeam => currentTeam.Id == teamId).FirstOrDefault();
            foreach (var member in team.Members)
            {
                System.Console.WriteLine(member.Id + "|" + member.displayName + "|" + member.uniqueName);
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
                        writer.WriteLine(member.uniqueName + "|" + member.displayName +  "|" + team.Name);
                    }
                }
            }
        }
    }
}
