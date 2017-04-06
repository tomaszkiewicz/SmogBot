

CREATE VIEW [Common].[Measurements]
AS
SELECT
    [c].[Name] AS CityName,
    [s].[Name] AS StationName,
    [m].[Time],
    [p].[Name] AS PollutantName,
    [m].[Value],
    [p].[Norm],
    [p].[Unit],
    [m].[Value] / [p].[Norm] AS PercentNorm,
    (
      SELECT
        MAX(AqiValue)
      FROM
        [dbo].[AqiLevels] al
      WHERE
        al.[PollutantId] = p.[Id]
        AND m.[Value] >= al.[LowerLevel]
    ) AS AqiValue
FROM
    [dbo].[Measurements] [m]
JOIN (
       SELECT
        [StationId],
        [PollutantId],
        MAX([Time]) AS [Time]
       FROM
        [dbo].[Measurements]
       GROUP BY
        [StationId],
        [PollutantId]
     ) [nm]
ON  [nm].[Time] = [m].[Time]
    AND [nm].[StationId] = [m].[StationId]
    AND [nm].[PollutantId] = [m].[PollutantId]
JOIN [dbo].[Stations] s
ON  [s].[Id] = [m].[StationId]
JOIN [dbo].[Cities] c
ON  [c].[Id] = [s].[CityId]
JOIN [dbo].[Pollutants] p
ON  [p].[Id] = [m].[PollutantId];