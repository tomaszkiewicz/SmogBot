






CREATE VIEW [Bot].[Measurements]
AS
    SELECT
        [c].[Name] AS CityName,
        [s].[Name] AS StationName,
        [m].[Time],
		[p].[Name] AS PollutantName,
		[m].[Value],
		[p].[Norm],
		[p].[Unit],
		[m].[Value] / [p].[Norm] AS PercentNorm
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
	ON [p].[Id] = [m].[PollutantId];