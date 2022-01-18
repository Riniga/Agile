using Agile.Library.Teams.Enum;
using System.Text.Json.Serialization;

namespace Agile.Library.Teams.Model
{
    public class Member
    {
        public Team Team { get; set; }
        public Role Role { get; set; }
        public Employee Employee { get; set; }
    }
}
