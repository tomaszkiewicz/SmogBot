
CREATE PROCEDURE [Updater].[UpdateAqiMeasurement]
    @cityName NVARCHAR(128),
    @stationName NVARCHAR(128),
    @time DATETIME,
    @value INT
AS
BEGIN
    SET NOCOUNT ON;

    EXEC [Updater].[EnsureStation] @cityName, @stationName;	

    DECLARE @stationId INT = [dbo].[GetStationIdByName](@cityName, @stationName);
	
    IF NOT EXISTS ( SELECT
                        m.*
                    FROM
                        [dbo].[AqiMeasurements] [m]
                    JOIN (
                           SELECT
                            [StationId],
                            MAX([Time]) AS [Time]
                           FROM
                            [dbo].[AqiMeasurements]
                           GROUP BY
                            [StationId]
                         ) [nm]
                    ON  [nm].[Time] = [m].[Time]
                        AND [nm].[StationId] = [m].[StationId]
                    WHERE
                        m.[StationId] = @stationId
                        AND m.[Value] = @value )
    BEGIN
        INSERT  INTO [dbo].[AqiMeasurements]
                ( [StationId], [Time], [Value] )
        VALUES
                ( @stationId, @time, @value );
    END;
END;