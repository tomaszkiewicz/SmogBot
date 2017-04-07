CREATE VIEW [dbo].[StationsAqi] AS
SELECT
	[m].[StationId],
    MAX([m].[AqiValue]) AS AqiValue
FROM
    [dbo].[LastMeasurements] m
GROUP BY m.[StationId]