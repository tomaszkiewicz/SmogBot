
CREATE PROCEDURE [Notifier].[GetNotifications]
    @lastTime DATETIME,
    @now DATETIME
AS
BEGIN
    SET NOCOUNT ON;
	
    SELECT
        u.[ChannelId],
        u.[FromId],
		u.[FromName],
        u.[ConversationId],
		u.[ConversationReference],
        src.[Time]
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
    WHERE
        src.[Time] > @lastTime
        AND src.[Time] < @now;
END;