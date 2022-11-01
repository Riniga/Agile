CREATE VIEW [dbo].[ViewRoles]
AS SELECT
	dbo.RoleInTeam.EmployeeId as EmployeeId,
	dbo.RoleInTeam.TeamId as TeamId,
	dbo.RoleInTeam.RoleId as RoleId,
	dbo.Roles.Name as RoleName
FROM dbo.RoleInTeam
INNER JOIN dbo.Roles ON dbo.RoleInTeam.RoleId = dbo.Roles.Id