MERGE INTO [dbo].[Pollutants] dst
USING (VALUES  
	('PM10', 50, N'µg/m³'),
	('PM2.5', 25, N'µg/m³'),
	('C6H6', 5, N'µg/m³'),
	('O3', 120, N'µg/m³'),
	('NO2', 200, N'µg/m³'),
	('SO2', 125, N'µg/m³'),
	('CO', 10000, N'mg/m³')
) AS src (Name, Norm, Unit) 
ON dst.Name = src.Name
WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Name],[Norm],[Unit]) VALUES (src.Name, src.Norm, src.Unit)
;