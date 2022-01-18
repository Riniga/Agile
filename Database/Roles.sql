CREATE TABLE [dbo].[Roles]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [RoleTypeId] INT NOT NULL, 
    CONSTRAINT [FK_Roles_RoleType] FOREIGN KEY ([RoleTypeId]) REFERENCES [RoleTypes]([Id])
)
