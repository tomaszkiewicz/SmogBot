
CREATE PROCEDURE [Bot].[EnableWarnings]
    @conversationId NVARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @userId INT = [dbo].[GetUserIdByConversationId](@conversationId);

	EXEC [Notifier].[UpdateWarnings] @userId;
	
END