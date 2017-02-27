
CREATE PROCEDURE [Bot].[AddNotificationTime]
    @conversationId NVARCHAR(128),
    @time NVARCHAR(5)
AS
BEGIN
	
    DECLARE @userId INT = [dbo].[GetUserIdByConversationId](@conversationId);

    IF @userId IS NULL
        THROW 50000, 'User not found', 1;

    IF NOT EXISTS ( SELECT
                        1
                    FROM
                        [dbo].[NotificationTimes]
                    WHERE
                        [UserId] = @userId
                        AND [Time] = @time )
        INSERT  INTO [dbo].[NotificationTimes]
                ([UserId], [Time])
        VALUES
                (@userId, @time);

END;