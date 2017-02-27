
CREATE PROCEDURE [Updater].[UpdateMeasurement]
    @cityName NVARCHAR(128),
	@stationName NVARCHAR(128),
    @time DATETIME,
    @pollutantName NVARCHAR(128),
    @value DECIMAL(9, 3)
AS
BEGIN
    SET NOCOUNT ON;

	EXEC [Updater].[EnsureStation] @cityName, @stationName;	

    DECLARE @stationId INT = [dbo].[GetStationIdByName](@cityName, @stationName);
    DECLARE @pollutantId INT = [dbo].[GetPollutantIdByName](@pollutantName);
	
    IF NOT EXISTS ( SELECT
                        m.*
                    FROM
                        [dbo].[Measurements] [m]
                    JOIN (
                           SELECT
                            [StationId],
                            [PollutantId],
                            MAX([Time]) AS [Time]
                           FROM
                            [dbo].[Measurements]
                           GROUP BY
                            [StationId],
                            [PollutantId]
                         ) [nm]
                    ON  [nm].[Time] = [m].[Time]
                        AND [nm].[StationId] = [m].[StationId]
                        AND [nm].[PollutantId] = [m].[PollutantId]
                    WHERE
                        m.[StationId] = @stationId
                        AND m.[PollutantId] = @pollutantId
                        AND m.[Value] = @value )
    BEGIN
        INSERT  INTO [dbo].[Measurements]
                (
                  [StationId],
                  [Time],
                  [PollutantId],
                  [Value]
                )
        VALUES
                (
                  @stationId,
                  @time,
                  @pollutantId,
                  @value
                );
    END;
END;