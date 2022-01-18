CREATE TABLE [dbo].[DeliveryPlannings]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	[Name] nvarchar(50) not null,
	[PlanningDate] Date not null,
	[StartDate] Date not null,
	[EndDate] Date not null,
)
