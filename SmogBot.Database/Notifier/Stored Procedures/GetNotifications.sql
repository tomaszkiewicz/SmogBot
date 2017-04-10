
CREATE PROCEDURE [Notifier].[GetNotifications]
    @lastTime DATETIME,
    @now DATETIME
AS
BEGIN
    SET NOCOUNT ON;
	
    SELECT
        u.[ConversationReference],
        c.[Name] AS CityName,
        MAX(src.[Time]) AS Time
    FROM
        (
          SELECT
            Id,
            UserId,
            DATEADD(D, DATEDIFF(D, 0, GETDATE()), CONVERT(DATETIME, [Time])) AS Time
          FROM
            [dbo].[NotificationTimes]
        ) src
    JOIN [dbo].[Users] u
    ON  [u].[Id] = [src].[UserId]
    JOIN [dbo].[UsersPreferences] up
    ON  [up].[UserId] = [u].[Id]
    JOIN [dbo].[Cities] c
    ON  [c].[Id] = [up].[CityId]
    WHERE
        src.[Time] > @lastTime
        AND src.[Time] <= @now
    GROUP BY
        [u].[ChannelId],
        [u].[FromId],
        [u].[FromName],
        [u].[ConversationId],
        [u].[ConversationReference],
        [c].[Name];
END;