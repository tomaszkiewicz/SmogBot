
CREATE FUNCTION [dbo].[LongestPrefix]
(
 @s NVARCHAR(128),
 @t NVARCHAR(128)
)
RETURNS INT
AS
BEGIN

    DECLARE @max INT;

    WITH    [series]([num])
              AS (
                  SELECT
                    1
                  UNION ALL
                  SELECT
                    [num] + 1
                  FROM
                    [series]
                  WHERE
                    [num] <= 100
                 )
        SELECT
            @max = MAX([num])
        FROM
            [series]
        WHERE
            LEFT(LOWER(@s), [num]) = LEFT(LOWER(@t), [num]);

    RETURN ISNULL(@max, 0);

END;