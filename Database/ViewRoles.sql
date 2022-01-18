CREATE VIEW [dbo].[ViewRoles]
AS SELECT
	dbo.Roles.Id as RoleId,
	dbo.Roles.Name as RoleName,
	dbo.RoleTypes.Name as RoleType
FROM dbo.Roles
INNER JOIN dbo.RoleTypes ON dbo.Roles.RoleTypeId = dbo.RoleTypes.Id