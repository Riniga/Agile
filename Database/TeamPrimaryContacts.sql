CREATE TABLE [dbo].[TeamPrimaryContacts]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	[TeamId] INT NOT NULL,
	[EmployeeId] INT NOT NULL, 
    CONSTRAINT [FK_TeamPrimaryContacts_Employees] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees]([Id]),
	CONSTRAINT [FK_TeamPrimaryContacts_Teams] FOREIGN KEY ([TeamId]) REFERENCES [Teams]([Id]),
)
