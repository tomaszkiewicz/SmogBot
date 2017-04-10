


CREATE VIEW [Notifier].[ActiveWarnings] AS
SELECT
	[u].[ConversationReference],
	[ci].[Name] AS [CityName],
	[lw].[StationId],
    [lw].[LastAqiValue],
    [sa].[AqiValue] AS [NewAqiLevel]
FROM
    [dbo].[LastWarnings] [lw]
JOIN [dbo].[StationsAqi] [sa]
ON  [sa].[StationId] = [lw].[StationId]
    AND [lw].[LastAqiValue] != [sa].[AqiValue]
JOIN [dbo].[Stations] [st]
ON  [st].[Id] = [lw].[StationId]
JOIN [dbo].[Cities] [ci]
ON  [ci].[Id] = [st].[CityId]
JOIN [dbo].[Users] u
ON [u].[Id] = [lw].[UserId]
WHERE [lw].[LastAqiValue] != [sa].[AqiValue]