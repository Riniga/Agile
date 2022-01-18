CREATE TABLE [dbo].[DeliveryPlanningParticipations]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [DeliveryPlanningId] INT NOT NULL, 
    [EmployeeId] INT NOT NULL, 
    [DeliveryPlanningParticipationScaleId] INT NOT NULL, 
    CONSTRAINT [FK_DeliveryPlanningParticipations_DeliveryPlanningParticipationScales] FOREIGN KEY ([DeliveryPlanningParticipationScaleId]) REFERENCES [DeliveryPlanningParticipationScales]([Id])
)
