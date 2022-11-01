using Agile.Library.Teams.Model;
using System.Collections.Generic;

namespace Agile.Library.Teams
{
    public class Employee
    {
        public Employee() { }
        public string Id { get; set; }
        public string uniqueName { get; set; }
        public string displayName { get; set; }
        public string url { get; set; }
        public string descriptor { get; set; }
        public string imageUrl { get; set; }
        public object _links { get; set; }
        //"_links": {"avatar": {"href": "https://dev.azure.com/skanskanordic/_apis/GraphProfile/MemberAvatars/aad.ZmZiZjBjMWItYjNiZi03NjM0LTlhNzgtMDEyYjZiZDNmYjU1"}},

        public List<RoleInTeam> RoleInTeam { get; set; }



    }
}
