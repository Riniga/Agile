CREATE TABLE [dbo].[Teams]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [TeamTypeId] INT NOT NULL, 
    CONSTRAINT [FK_Teams_TeamTypes] FOREIGN KEY ([TeamTypeId]) REFERENCES [TeamTypes]([Id])
)
