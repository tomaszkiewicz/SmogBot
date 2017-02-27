
CREATE FUNCTION [dbo].[GetCityIdByName]
(
	@name NVARCHAR(128)
)
RETURNS INT
AS
BEGIN
	
	RETURN (SELECT Id FROM [dbo].[Cities] WHERE [Name] = @name);

END