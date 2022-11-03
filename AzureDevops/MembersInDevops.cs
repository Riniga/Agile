using Agile.Library.Teams;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace AzureDevops
{
    internal class MembersInDevops : Devops
    {
        public MembersInDevops(ILogger<Devops> log, IConfiguration config) : base(log, config)  {}

        public override void Run()
        {
            var allTeams = Teams.Instance.All;
            //Todo: Write to -> Settings.ExcelFile

            foreach (var team in allTeams)
            {
                foreach (var member in team.Members) Console.WriteLine(member.uniqueName+ ";" + team.name);
            }
        }
    }
}
