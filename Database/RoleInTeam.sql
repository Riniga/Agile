CREATE TABLE [dbo].[RoleInTeam]
(
    [EmployeeId] varchar(64) NOT NULL, 
    [TeamId] varchar(64) NOT NULL, 
    [RoleId] INT NOT NULL, 
    CONSTRAINT [PK_RoleInTeam] PRIMARY KEY ([EmployeeId], [TeamId]), 
    CONSTRAINT [FK_RoleInTeam_Roles] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([Id])
)
