
CREATE PROCEDURE [Bot].[DeleteNotificationTime]
    @conversationId NVARCHAR(128),
    @time NVARCHAR(5)
AS
BEGIN
	
    DECLARE @userId INT = [dbo].[GetUserIdByConversationId](@conversationId);

    IF @userId IS NULL
        THROW 50000, 'User not found', 1;

    DELETE FROM
        [dbo].[NotificationTimes]
    WHERE
        [UserId] = @userId
        AND [Time] = @time;
    
END;