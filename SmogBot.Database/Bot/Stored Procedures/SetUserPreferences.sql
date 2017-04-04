
CREATE PROCEDURE [Bot].[SetUserPreferences]
    @channelId NVARCHAR(128),
    @fromId NVARCHAR(128),
    @fromName NVARCHAR(128),
    @conversationId NVARCHAR(128),
    @cityName NVARCHAR(128)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @userId INT = [dbo].[GetUserIdByConversationId](@conversationId);
    DECLARE @cityId INT = [dbo].[GetCityIdByName](@cityName);

    MERGE INTO [dbo].[UsersPreferences] [dst]
    USING ( VALUES
        (@cityId)
		) AS [src] ([CityId])
    ON [dst].[UserId] = @userId
    WHEN NOT MATCHED THEN
        INSERT
               ([UserId], [CityId])
        VALUES (
                @userId,
                [src].[CityId]
               )
    WHEN MATCHED THEN
        UPDATE SET
               [dst].[CityId] = [src].[CityId];
END;