
CREATE FUNCTION [dbo].[GetPollutantIdByName]
(
	@name NVARCHAR(128)
)
RETURNS INT
AS
BEGIN
	
	RETURN (SELECT Id FROM [dbo].[Pollutants] WHERE [Name] = @name);

END