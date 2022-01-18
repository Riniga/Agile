CREATE VIEW [dbo].[ViewEmployees]
AS SELECT
	dbo.Employees.Id as EmployeeId,
	dbo.Employees.Email as EmployeeEmail,
	dbo.Employees.Firstname as EmployeeFirstname,
	dbo.Employees.Lastname as EmployeeLastname,
	dbo.Employees.Notes as EmployeeNotes,
	dbo.Employees.InDevops as EmployeeInDevops,
	dbo.Employees.InTeams as EmployeeInTeams,
	dbo.Roles.Id as RoleId,
	dbo.Roles.Name as RoleName,
	dbo.Teams.Id as TeamId,
	dbo.Teams.Name as TeamName
FROM dbo.Employees
INNER JOIN dbo.RoleInTeam ON dbo.Employees.Id = dbo.RoleInTeam.EmployeeId
INNER JOIN dbo.Roles ON dbo.RoleInTeam.RoleId = dbo.Roles.Id
INNER JOIN dbo.Teams ON dbo.RoleInTeam.TeamId = dbo.Teams.Id
