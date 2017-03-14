CREATE VIEW [Bot].[StationsAqi]
AS
SELECT
    ci.[Name] AS CityName,
    st.[Name] AS StationName,
    [data].[Time],
    [data].[Value]
FROM
    (
      SELECT
        am.*
      FROM
        [dbo].[AqiMeasurements] am
      JOIN
        (
          SELECT
            StationId,
            MAX(Time) AS Time
          FROM
            [dbo].[AqiMeasurements]
          GROUP BY
            [StationId]
        ) mm
      ON
        am.[Time] = mm.[Time]
        AND am.[StationId] = mm.[StationId]
    ) data
JOIN [dbo].[Stations] st
ON  [st].[Id] = [data].[StationId]
JOIN [dbo].[Cities] ci
ON  [ci].[Id] = [st].[CityId];