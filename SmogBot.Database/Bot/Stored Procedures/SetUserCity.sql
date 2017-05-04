
CREATE PROCEDURE [Bot].[SetUserCity]
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

    UPDATE
        [dbo].[UsersPreferences]
    SET
        [CityId] = @cityId
    WHERE
        [UserId] = @userId;

    IF @@ROWCOUNT = 0
    BEGIN
        INSERT  INTO [dbo].[UsersPreferences]
                ([UserId], [CityId])
        VALUES
                (@userId, @cityId);
    END;
    
END;