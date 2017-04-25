
CREATE PROCEDURE [Bot].[AddFeedback]
    @conversationId NVARCHAR(128),
    @message NVARCHAR(MAX),
	@version NVARCHAR(16)
AS
BEGIN
	
    DECLARE @userId INT = [dbo].[GetUserIdByConversationId](@conversationId);

    IF @userId IS NULL
        THROW 50000, 'User not found', 1;

    INSERT  INTO [dbo].[Feedback]
            (
              [UserId],
              [CreatedTime],
              [Message],
			  [Version]
            )
    VALUES
            (
              @userId,
              GETUTCDATE(),
              @message,
			  @version
            );

END;