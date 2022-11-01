using Agile.Library.Teams.Enum;
using Agile.Library.Teams.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Agile.Library.Teams
{
    public sealed class Roles
    {
        private static readonly Lazy<Roles> lazy = new Lazy<Roles>(() => new Roles());
        public static Roles Instance { get { return lazy.Value; } }

        public List<MemberRoleInTeam> All;
        private Roles()
        {
            All = GetRolesAsync().Result;
        }
        //public void AddRole(Role role)
        //{
        //    if (All.Where(role2 => role2.Name == role.Name).Count() > 0) return;
        //    var data = Database.GetDataSetAsync("SELECT Id FROM [Roles] WHERE [Name]='" + role.Name+ "'").Result;
        //    if (data.Tables[0].Rows.Count > 0) return;
        //    var task = Task.Run(async () => await Database.ExecuteCommandAsync("INSERT INTO [Roles] ([Name],[RoleTypeId]) VALUES ('" + role.Name + "','" + (int)role.RoleType + "') "));
        //    var id = task.Result;
        //    if (id != 0) All.Add(role);
        //}


        private async Task<List<MemberRoleInTeam>> GetRolesAsync()
        {
            var roles = new List<MemberRoleInTeam>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT EmployeeId, TeamId, RoleId, RoleName FROM ViewRoles");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    var role = new Role() { Id = (int)row["RoleId"], Name = (string)row["RoleName"] };
                    roles.Add(new MemberRoleInTeam { TeamId = (string)row["TeamId"],MemberId= (string)row["EmployeeId"], Role=role });
                }
            }
            return roles;
        }
        public static async Task<List<Role>> GetRolesAsync(Employee employee)
        {
            var employeeRoles = new List<Role>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT RoleId, RoleName, RoleType FROM ViewEmployeeRoles WHERE EmployeeId='{employee.Id}'");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    employeeRoles.Add(new Role { Id = (int)row["RoleId"], Name = (string)row["RoleName"], RoleType = (RoleTypes)System.Enum.Parse(typeof(RoleTypes), (string)row["RoleType"]) });
                }
            }
            return employeeRoles;
        }
        public static async Task<Role> GetRoleAsync(int Id)
        {
            Role role = null;
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT RoleId, RoleName, RoleType FROM ViewRoles WHERE RoleId ='{Id}'");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    role = new Role
                    {
                        Id = (int)row["RoleId"],
                        Name = (string)row["RoleName"],
                        RoleType = (RoleTypes)System.Enum.Parse(typeof(RoleTypes), (string)row["RoleType"])
                    };
                }
            }
            if (role == null) throw new Exception($"Role with id {Id} not found");
            return role;
        }

        public async Task<int> CreateRoleAsync(Role role)
        {
            //var debtId = await Database.ExecuteCommandAsync($"INSERT INTO Debts (ContractId, PersonId) OUTPUT INSERTED.ID VALUES('{contract.Id}','{person.Id}')");
            //await Database.ExecuteCommandAsync($"INSERT INTO Transactions (DebtId, Date, Type, Amount) VALUES('{debtId}','{DateTime.Now}','{TransactionType.SetBalance}','{amount}')");
            return await Database.ExecuteCommandAsync($"EXEC sp_CreateRole @RoleId='{role.Id}',@RoleName='{role.Name}',@RoleType='{role.RoleType}'");
        }
    }
}
