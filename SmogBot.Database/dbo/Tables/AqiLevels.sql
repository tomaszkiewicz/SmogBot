CREATE TABLE [dbo].[AqiLevels] (
    [Id]          INT     IDENTITY (1, 1) NOT NULL,
    [PollutantId] INT     NOT NULL,
    [LowerLevel]  INT     NOT NULL,
    [AqiValue]    TINYINT NOT NULL,
    CONSTRAINT [PK_AqiLevels] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AqiLevels_Pollutants] FOREIGN KEY ([PollutantId]) REFERENCES [dbo].[Pollutants] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_AqiLevels]
    ON [dbo].[AqiLevels]([PollutantId] ASC, [LowerLevel] ASC);

