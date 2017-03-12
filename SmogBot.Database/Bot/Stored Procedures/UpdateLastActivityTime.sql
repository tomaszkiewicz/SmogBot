
CREATE PROCEDURE [Bot].[UpdateLastActivityTime]
    @channelId NVARCHAR(128),
    @fromId NVARCHAR(128),
    @fromName NVARCHAR(128),
    @conversationId NVARCHAR(128)
AS
BEGIN

    EXEC [Bot].[EnsureUser] @channelId, @fromId, @fromName, @conversationId;
	
    UPDATE
        [dbo].[Users]
    SET
        [LastActivityTime] = GETUTCDATE()
    WHERE
        [ChannelId] = @channelId
        AND [FromId] = @fromId
        AND [FromName] = @fromName
        AND [ConversationId] = @conversationId;

END;