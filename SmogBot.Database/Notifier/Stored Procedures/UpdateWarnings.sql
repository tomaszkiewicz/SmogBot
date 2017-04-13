
CREATE PROCEDURE [Notifier].[UpdateWarnings] 
	@userId INT
AS
BEGIN
    SET NOCOUNT ON;
	    
    MERGE INTO [dbo].[LastWarnings] dst
    USING
        (
          SELECT
            sa.*
          FROM
            [dbo].[StationsAqi] sa
          JOIN
            [dbo].[Stations] s
          ON
            sa.[StationId] = s.[Id]
          JOIN
            [dbo].[UsersPreferences] up
          ON
            [up].[CityId] = [s].[CityId]
            AND [up].[UserId] = @userId
        ) src
    ON [dst].[StationId] = src.[StationId]
        AND dst.[UserId] = @userId
    WHEN MATCHED THEN
        UPDATE SET
               [dst].[LastAqiValue] = [src].[AqiValue]
    WHEN NOT MATCHED THEN
        INSERT
               (
                 [UserId],
                 [StationId],
                 [LastAqiValue]
               )
        VALUES (
                 @userId,
                 [src].[StationId],
                 [src].[AqiValue]
               );
END;