using Agile.Library.Teams.Enum;
using Agile.Library.Teams.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Agile.Library.Teams
{
    public static class Roles
    {
        
        public static async Task<List<Role>> GetRolesAsync()
        {
            var roles = new List<Role>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT RoleId, RoleName, RoleType FROM ViewRoles");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    roles.Add(new Role { Id = (int)row["RoleId"], Name = (string)row["RoleName"], RoleType = (RoleTypes)System.Enum.Parse(typeof(RoleTypes), (string)row["RoleType"]) });
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

        public static async Task<int> CreateRoleAsync(Role role)
        {
            //var debtId = await Database.ExecuteCommandAsync($"INSERT INTO Debts (ContractId, PersonId) OUTPUT INSERTED.ID VALUES('{contract.Id}','{person.Id}')");
            //await Database.ExecuteCommandAsync($"INSERT INTO Transactions (DebtId, Date, Type, Amount) VALUES('{debtId}','{DateTime.Now}','{TransactionType.SetBalance}','{amount}')");
            return await Database.ExecuteCommandAsync($"EXEC sp_CreateRole @RoleId='{role.Id}',@RoleName='{role.Name}',@RoleType='{role.RoleType}'");
        }
    }
}
