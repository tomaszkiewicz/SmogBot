CREATE VIEW [Bot].[UsersPreferences] AS
SELECT
    [u].[ChannelId],
    [u].[FromId],
    [u].[FromName],
    [u].[ConversationId],
    [ci].[Name] AS [CityName]
FROM
    [dbo].[UsersPreferences] [up]
JOIN [dbo].[Cities] [ci]
ON  [ci].[Id] = [up].[CityId]
JOIN [dbo].[Users] [u]
ON  [u].[Id] = [up].[UserId];