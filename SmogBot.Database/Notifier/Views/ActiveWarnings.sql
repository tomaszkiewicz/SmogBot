CREATE VIEW [Notifier].[ActiveWarnings] AS
SELECT
    [lw].[UserId],
    [lw].[StationId],
    [lw].[LastAqiLevel],
    [sa].[AqiValue] AS [NewAqiLevel]
FROM
    [dbo].[LastWarnings] lw
JOIN [dbo].[StationsAqi] sa
ON  [sa].[StationId] = [lw].[StationId]
    AND [lw].[LastAqiLevel] != [sa].[AqiValue];