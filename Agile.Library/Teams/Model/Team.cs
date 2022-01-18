using Agile.Library.Teams.Enum;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Agile.Library.Teams.Model
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TeamTypes TeamType { get; set; }
    }
}
