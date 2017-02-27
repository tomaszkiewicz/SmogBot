CREATE TABLE [dbo].[AqiMeasurements] (
    [Id]        INT      IDENTITY (1, 1) NOT NULL,
    [StationId] INT      NOT NULL,
    [Time]      DATETIME NOT NULL,
    [Value]     INT      NOT NULL,
    CONSTRAINT [PK_AqiMeasurements] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AqiMeasurements_Stations] FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AqiMeasurements]
    ON [dbo].[AqiMeasurements]([StationId] ASC, [Time] ASC);

