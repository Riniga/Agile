using Agile.Library.Teams;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace AzureDevops
{
    internal class ProjectAdmins : Devops
    {
        public ProjectAdmins(ILogger<Devops> log, IConfiguration config) : base(log, config)  {}

        public override void Run()
        {
            var allProjects = Projects.Instance.All;
            foreach (var project in allProjects)
            {
                Console.WriteLine(project.name + ":" + project.id);
                //foreach (var member in project.Admins) Console.WriteLine(member.uniqueName+ ";" + project.name);
            }
        }
    }
}
