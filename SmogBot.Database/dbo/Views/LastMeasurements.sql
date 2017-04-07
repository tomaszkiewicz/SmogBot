CREATE VIEW [dbo].[LastMeasurements] AS
SELECT
	[m].[StationId],
	[m].[PollutantId],
	[p].[Name] AS PollutantName,
    [m].[Time],
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
        al.[PollutantId] = m.[PollutantId]
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
JOIN [dbo].[Pollutants] p
ON [p].[Id] = [m].[PollutantId]