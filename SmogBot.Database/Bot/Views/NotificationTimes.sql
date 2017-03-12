
CREATE VIEW [Bot].[NotificationTimes]
AS
SELECT
    [u].[ConversationId],
    [nt].[Time]
FROM
    [dbo].[NotificationTimes] [nt]
JOIN [dbo].[Users] [u]
ON  [u].[Id] = [nt].[UserId];