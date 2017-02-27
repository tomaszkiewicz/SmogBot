
CREATE FUNCTION [dbo].[ReplacePolishCharacters]
(
	@s NVARCHAR(128)
)
RETURNS NVARCHAR(128)
AS
BEGIN
	
	SELECT @s = REPLACE(@s, N'ł', N'l');
	SELECT @s = REPLACE(@s, N'ó', N'o');
	SELECT @s = REPLACE(@s, N'ź', N'z');
	SELECT @s = REPLACE(@s, N'ż', N'z');
	SELECT @s = REPLACE(@s, N'ę', N'e');
	SELECT @s = REPLACE(@s, N'ą', N'a');
	SELECT @s = REPLACE(@s, N'ń', N'n');
	SELECT @s = REPLACE(@s, N'ś', N's');
	SELECT @s = REPLACE(@s, N'ć', N'c');
	
	SELECT @s = REPLACE(@s, N'Ł', N'L');
	SELECT @s = REPLACE(@s, N'Ó', N'O');
	SELECT @s = REPLACE(@s, N'Ź', N'Z');
	SELECT @s = REPLACE(@s, N'Ż', N'Z');
	SELECT @s = REPLACE(@s, N'Ę', N'E');
	SELECT @s = REPLACE(@s, N'Ą', N'A');
	SELECT @s = REPLACE(@s, N'Ń', N'N');
	SELECT @s = REPLACE(@s, N'Ś', N'S');
	SELECT @s = REPLACE(@s, N'Ć', N'C');
	
	RETURN @s;

END