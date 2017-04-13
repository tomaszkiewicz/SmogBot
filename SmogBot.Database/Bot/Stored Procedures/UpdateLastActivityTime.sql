
CREATE PROCEDURE [Bot].[UpdateLastActivityTime]
    @channelId NVARCHAR(128),
    @fromId NVARCHAR(128),
    @fromName NVARCHAR(128),
    @conversationId NVARCHAR(128),
	@conversationReference NVARCHAR(1024)
AS
BEGIN

    EXEC [Bot].[EnsureUser] @channelId, @fromId, @fromName, @conversationId, @conversationReference;
	
    UPDATE
        [dbo].[Users]
    SET
        [LastActivityTime] = GETUTCDATE(),
		[ConversationReference] = @conversationReference
    WHERE
        [ChannelId] = @channelId
        AND [FromId] = @fromId
        AND [FromName] = @fromName
        AND [ConversationId] = @conversationId;

END;