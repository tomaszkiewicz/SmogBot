CREATE TABLE [dbo].[LastWarnings] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [UserId]       INT NOT NULL,
    [StationId]    INT NOT NULL,
    [LastAqiLevel] INT NOT NULL,
    CONSTRAINT [PK_LastWarnings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LastWarnings_Stations] FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations] ([Id]),
    CONSTRAINT [FK_LastWarnings_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_LastWarnings]
    ON [dbo].[LastWarnings]([UserId] ASC, [StationId] ASC);

