CREATE TABLE [dbo].[Measurements] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [StationId]   INT            NOT NULL,
    [Time]        DATETIME       NOT NULL,
    [PollutantId] INT            NOT NULL,
    [Value]       DECIMAL (9, 3) NOT NULL,
    CONSTRAINT [PK_Measurements] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Measurements_Pollutants] FOREIGN KEY ([PollutantId]) REFERENCES [dbo].[Pollutants] ([Id]),
    CONSTRAINT [FK_Measurements_Stations] FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations] ([Id]),
    CONSTRAINT [IX_Measurements] UNIQUE NONCLUSTERED ([StationId] ASC, [Time] ASC, [PollutantId] ASC)
);







