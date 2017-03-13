
CREATE PROCEDURE [Bot].[EnsureUser]
    @channelId NVARCHAR(128),
    @fromId NVARCHAR(128),
    @fromName NVARCHAR(128),
    @conversationId NVARCHAR(128),
	@conversationReference NVARCHAR(1024)
AS
BEGIN
	
    IF NOT EXISTS ( SELECT
                        1
                    FROM
                        [dbo].[Users]
                    WHERE
                        [ChannelId] = @channelId
                        AND [FromId] = @fromId
                        AND [FromName] = @fromName
                        AND [ConversationId] = @conversationId )
        INSERT  INTO [dbo].[Users]
                (
                 [ChannelId],
                 [FromId],
                 [FromName],
                 [ConversationId],
				 [ConversationReference]
	            )
        VALUES
                (
                 @channelId,
                 @fromId,
                 @fromName,
                 @conversationId,
				 @conversationReference
	            );

END;