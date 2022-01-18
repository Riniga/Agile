using Agile.Library.Teams.Model;
using System.Collections.Generic;

namespace Agile.Library.Teams
{
    public class Employee
    {
        public Employee() { }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Notes { get; set; }
        public bool InDevops { get; set; }
        public bool InTeaams{ get; set; }
    }
}
