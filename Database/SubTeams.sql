CREATE TABLE [dbo].[SubTeams]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	[Name] nvarchar(50) NOT NULL,
	[TeamId] int NOT NULL, 
    CONSTRAINT [FK_SubTeams_Teams] FOREIGN KEY ([TeamId]) REFERENCES [Teams]([Id])
)
