
CREATE FUNCTION [dbo].[GetUserIdByConversationId]
(
	@conversationId NVARCHAR(128)
)
RETURNS INT
AS
BEGIN
	
	RETURN (SELECT Id FROM [dbo].[Users] WHERE [ConversationId] = @conversationId);

END