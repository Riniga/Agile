using Agile.Library.Teams.Enum;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Agile.Library.Teams.Model
{
    public class Team
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string url;
        public string description;
        public string identityUrl;
        public string projectName;
        public string projectId;

        public List<Employee> Members { get; set; }
    }
}
