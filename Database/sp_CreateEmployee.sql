CREATE PROCEDURE [dbo].[sp_CreateEmployee]
	@Email nvarchar(50),
	@Firstname nvarchar(50), 
	@Lastname nvarchar(50),
	@RoleId int,
	@TeamId int
AS
	declare @employeeId int;
	INSERT INTO Employees (Email, Firstname, Lastname) 
	VALUES(@Email, @Firstname, @Lastname)
	SET @employeeId = scope_identity()
	
	INSERT INTO RoleInTeam (EmployeeId, RoleId, TeamId) 
	VALUES(@employeeId, @RoleId, @TeamId)

	
RETURN 0
