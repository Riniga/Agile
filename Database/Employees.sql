CREATE TABLE [dbo].[Employees]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Email] NVARCHAR(50) NOT NULL, 
    [Firstname] NVARCHAR(50) NOT NULL, 
    [Lastname] NVARCHAR(50) NOT NULL, 
    [Notes] TEXT NULL, 
    [InDevops] BIT NULL, 
    [InTeams] BIT NULL
)
