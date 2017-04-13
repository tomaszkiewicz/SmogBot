CREATE VIEW [Bot].[WarningsStatus] AS
SELECT
    u.[ConversationId], COUNT([lw].[UserId]) AS WarningsEnabled
FROM
    [dbo].[Users] [u]
LEFT JOIN (
           SELECT DISTINCT
            [UserId]
           FROM
            [dbo].[LastWarnings]
          ) [lw]
ON  [lw].[UserId] = [u].[Id]
GROUP BY [u].[ConversationId]