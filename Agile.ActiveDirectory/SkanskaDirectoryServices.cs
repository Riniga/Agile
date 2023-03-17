using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace ActiveDirectory
{
    class SkanskaDirectoryServices
    {
        public void PrintGroupMembers(string group)
        {
            var groups = GetGroup(group);
            GroupPrincipal currentGroup = (GroupPrincipal)groups.FirstOrDefault();
            var members = currentGroup.GetMembers(false);
            foreach (var member in members)
            {
                UserPrincipal user = (UserPrincipal)member;
                PrintRow(user);
            }
        }
        private PrincipalSearchResult<Principal> GetGroup(string groupName)
        {
            GroupPrincipal findAllGroups = new GroupPrincipal(ActiveDirectory, groupName);
            PrincipalSearcher ps = new PrincipalSearcher(findAllGroups);
            return ps.FindAll();
        }
        private void PrintRow(UserPrincipal user)
        {
            Console.Write($"{user.SamAccountName}|");
            Console.Write($"{user.DisplayName}|");
            Console.Write($"{user.GetProperty("title")}|");
            Console.Write($"{user.EmailAddress}|");
            Console.Write($"{user.VoiceTelephoneNumber}|");
            Console.Write($"{user.GetProperty("department")}|");
            Console.Write($"{user.GetProperty("physicalDeliveryOfficeName")}|");
            Console.Write($"{user.GetProperty("company")}|");
            var mananger = user.GetProperty("manager").Replace("\\", "").Replace("CN=", "");
            try { mananger = mananger.Remove(mananger.IndexOf(",OU=")); } catch (Exception ex) { }
            Console.WriteLine($"{mananger}");
        }

        private PrincipalContext ActiveDirectory
        {
            get
            {
                var activeDirectory = new PrincipalContext(ContextType.Domain, "skanska.org");
                return activeDirectory;
            }
        }
    }
}
