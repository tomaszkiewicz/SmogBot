
CREATE FUNCTION [dbo].[GetStationIdByName]
(
	@cityName NVARCHAR(128),
	@stationName NVARCHAR(128)
)
RETURNS INT
AS
BEGIN
	
	RETURN (SELECT Id FROM [dbo].[Stations] WHERE [Name] = @stationName AND [CityId] = [dbo].[GetCityIdByName](@cityName));

END