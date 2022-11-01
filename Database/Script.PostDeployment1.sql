/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

INSERT INTO Roles (name,RoleTypeId) VALUES('Scrum Master')
INSERT INTO Roles (name,RoleTypeId) VALUES('Architect')
INSERT INTO Roles (name,RoleTypeId) VALUES('Team')
INSERT INTO Roles (name,RoleTypeId) VALUES('Business Owner')
INSERT INTO Roles (name,RoleTypeId) VALUES('Product Management')
INSERT INTO Roles (name,RoleTypeId) VALUES('Product Owner')
INSERT INTO Roles (name,RoleTypeId) VALUES('STE')
INSERT INTO Roles (name,RoleTypeId) VALUES('Arkitekt')
INSERT INTO Roles (name,RoleTypeId) VALUES('ITSM')
INSERT INTO Roles (name,RoleTypeId) VALUES('ITSO')
INSERT INTO Roles (name,RoleTypeId) VALUES('Processledare')
INSERT INTO Roles (name,RoleTypeId) VALUES('Processpecialist')
INSERT INTO Roles (name,RoleTypeId) VALUES('Processägare')
INSERT INTO Roles (name,RoleTypeId) VALUES('Produktägare')
INSERT INTO Roles (name,RoleTypeId) VALUES('SR')