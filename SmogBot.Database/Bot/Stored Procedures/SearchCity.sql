
CREATE PROCEDURE [Bot].[SearchCity]
    @searchCity NVARCHAR(128)
AS
BEGIN
    SET NOCOUNT ON;

    WITH    data
              AS (
                   SELECT
                    *,
                    [dbo].[LongestPrefix]([dbo].[ReplacePolishCharacters](Name), [dbo].[ReplacePolishCharacters](@searchCity)) AS Score
                   FROM
                    [dbo].[Cities]
                 )
        SELECT
            Name
        FROM
            [data] d1
        JOIN (
               SELECT
                MAX(Score) AS MaxScore
               FROM
                [data]
             ) d2
        ON  d1.[Score] = d2.[MaxScore]
        WHERE
            d1.[Score] > 0;
END;