MERGE INTO [dbo].[AqiLevels] dst
USING (VALUES  
	([dbo].[GetPollutantIdByName]('PM10'), 0, 0),
	([dbo].[GetPollutantIdByName]('PM10'), 21, 1),
	([dbo].[GetPollutantIdByName]('PM10'), 61, 2),
	([dbo].[GetPollutantIdByName]('PM10'), 101, 3),
	([dbo].[GetPollutantIdByName]('PM10'), 141, 4),
	([dbo].[GetPollutantIdByName]('PM10'), 201, 5),

	([dbo].[GetPollutantIdByName]('PM2.5'), 0, 0),
	([dbo].[GetPollutantIdByName]('PM2.5'), 13, 1),
	([dbo].[GetPollutantIdByName]('PM2.5'), 37, 2),
	([dbo].[GetPollutantIdByName]('PM2.5'), 61, 3),
	([dbo].[GetPollutantIdByName]('PM2.5'), 85, 4),
	([dbo].[GetPollutantIdByName]('PM2.5'), 121, 5),
	
	([dbo].[GetPollutantIdByName]('O3'), 0, 0),
	([dbo].[GetPollutantIdByName]('O3'), 31, 1),
	([dbo].[GetPollutantIdByName]('O3'), 71, 2),
	([dbo].[GetPollutantIdByName]('O3'), 121, 3),
	([dbo].[GetPollutantIdByName]('O3'), 161, 4),
	([dbo].[GetPollutantIdByName]('O3'), 241, 5),
	
	([dbo].[GetPollutantIdByName]('NO2'), 0, 0),
	([dbo].[GetPollutantIdByName]('NO2'), 41, 1),
	([dbo].[GetPollutantIdByName]('NO2'), 101, 2),
	([dbo].[GetPollutantIdByName]('NO2'), 151, 3),
	([dbo].[GetPollutantIdByName]('NO2'), 201, 4),
	([dbo].[GetPollutantIdByName]('NO2'), 401, 5),
	
	([dbo].[GetPollutantIdByName]('SO2'), 0, 0),
	([dbo].[GetPollutantIdByName]('SO2'), 51, 1),
	([dbo].[GetPollutantIdByName]('SO2'), 101, 2),
	([dbo].[GetPollutantIdByName]('SO2'), 201, 3),
	([dbo].[GetPollutantIdByName]('SO2'), 351, 4),
	([dbo].[GetPollutantIdByName]('SO2'), 501, 5),
	
	([dbo].[GetPollutantIdByName]('C6H6'), 0, 0),
	([dbo].[GetPollutantIdByName]('C6H6'), 6, 1),
	([dbo].[GetPollutantIdByName]('C6H6'), 11, 2),
	([dbo].[GetPollutantIdByName]('C6H6'), 16, 3),
	([dbo].[GetPollutantIdByName]('C6H6'), 21, 4),
	([dbo].[GetPollutantIdByName]('C6H6'), 51, 5),
	
	([dbo].[GetPollutantIdByName]('CO'), 0, 0),
	([dbo].[GetPollutantIdByName]('CO'), 3, 1),
	([dbo].[GetPollutantIdByName]('CO'), 7, 2),
	([dbo].[GetPollutantIdByName]('CO'), 11, 3),
	([dbo].[GetPollutantIdByName]('CO'), 15, 4),
	([dbo].[GetPollutantIdByName]('CO'), 21, 5)
) AS src (PollutantId, LowerLevel, AqiValue) 
ON dst.[PollutantId] = src.[PollutantId] AND dst.[LowerLevel] = src.[LowerLevel]
WHEN NOT MATCHED BY TARGET THEN
	INSERT (PollutantId, LowerLevel, AqiValue)  VALUES (src.[PollutantId], src.[LowerLevel], src.[AqiValue])
;