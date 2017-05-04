



CREATE VIEW [Common].[Measurements]
AS
SELECT
    [c].[Name] AS CityName,
    [s].[Name] AS StationName,
	[s].[Id] AS StationId,
    [m].[Time],
    [m].[PollutantName],
    [m].[Value],
    [m].[Norm],
    [m].[Unit],
    [m].[PercentNorm],
    [m].[AqiValue]
FROM
    [dbo].[LastMeasurements] m
JOIN [dbo].[Stations] s
ON  [s].[Id] = [m].[StationId]
JOIN [dbo].[Cities] c
ON  [c].[Id] = [s].[CityId];