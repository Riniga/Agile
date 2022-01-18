CREATE TABLE [dbo].[RoleInTeam]
(
    [EmployeeId] INT NOT NULL, 
    [TeamId] INT NOT NULL, 
    [RoleId] INT NOT NULL, 
    CONSTRAINT [PK_RoleInTeam] PRIMARY KEY ([EmployeeId], [TeamId]), 
    CONSTRAINT [FK_RoleInTeam_Employees] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees]([Id]), 
    CONSTRAINT [FK_RoleInTeam_Teams] FOREIGN KEY ([TeamId]) REFERENCES [Teams]([Id]), 
    CONSTRAINT [FK_RoleInTeam_Roles] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([Id])
)
