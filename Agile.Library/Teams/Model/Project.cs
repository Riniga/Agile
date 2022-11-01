using Agile.Library.Teams.Enum;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Agile.Library.Teams.Model
{
    public class Project
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url;
        public string description;
        public string state;
        public string revision;
        public string visibility;
        public string lastUpdateTime;

        public List<Employee> Admins { get; set; }
    }
}
