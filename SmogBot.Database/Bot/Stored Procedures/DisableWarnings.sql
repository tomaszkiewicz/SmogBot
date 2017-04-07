
CREATE PROCEDURE [Bot].[DisableWarnings]
    @conversationId NVARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @userId INT = [dbo].[GetUserIdByConversationId](@conversationId);

	DELETE FROM [dbo].[LastWarnings] WHERE [UserId] = @userId;
	
END