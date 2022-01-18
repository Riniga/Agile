using Agile.Library.Teams.Enum;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Agile.Library.Teams.Model
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public RoleTypes RoleType { get; set; }
    }
}
