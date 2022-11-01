CREATE TABLE [dbo].[CourseInvitations]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [EmployeeId] INT NOT NULL, 
    [CourseId] INT NOT NULL, 
    [Date] DATE NOT NULL,
    CONSTRAINT [FK_CourseInvitations_Courses] FOREIGN KEY ([CourseId]) REFERENCES [Courses]([Id])
)
