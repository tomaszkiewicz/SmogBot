
CREATE PROCEDURE [Updater].[EnsureStation]
    @cityName NVARCHAR(128),
    @stationName NVARCHAR(128)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS ( SELECT
                        1
                    FROM
                        [dbo].[Cities]
                    WHERE
                        [Name] = @cityName )
    BEGIN
        INSERT  INTO [dbo].[Cities]
                ( [Name] )
        VALUES
                ( @cityName );
    END;

    DECLARE @cityId INT = [dbo].[GetCityIdByName](@cityName);

    IF NOT EXISTS ( SELECT
                        1
                    FROM
                        [dbo].[Stations]
                    WHERE
                        [Name] = @stationName
                        AND [CityId] = @cityId )
    BEGIN
        INSERT  INTO [dbo].[Stations]
                (
                  [CityId],
                  [Name]
                )
        VALUES
                (
                  [dbo].[GetCityIdByName](@cityName),
                  @stationName
		        );
    END;
END;