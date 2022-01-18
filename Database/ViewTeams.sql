CREATE VIEW [dbo].[ViewTeams]
AS SELECT
	dbo.Teams.Id as TeamId,
	dbo.Teams.Name as TeamName,
	dbo.TeamTypes.Name as TeamType
FROM dbo.Teams
INNER JOIN dbo.TeamTypes ON dbo.Teams.TeamTypeId = dbo.TeamTypes.Id