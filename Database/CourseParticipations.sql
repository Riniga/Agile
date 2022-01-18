CREATE TABLE [dbo].[CourseParticipations]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [EmployeeId] INT NOT NULL, 
    [CourseId] INT NOT NULL, 
    [Date] DATE NOT NULL, 
    CONSTRAINT [FK_CourseParticipations_Employees] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees]([Id]),
    CONSTRAINT [FK_CourseParticipations_Courses] FOREIGN KEY ([CourseId]) REFERENCES [Courses]([Id])
)

