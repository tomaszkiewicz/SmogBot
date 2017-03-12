CREATE TABLE [dbo].[NotificationTimes] (
    [Id]     INT          IDENTITY (1, 1) NOT NULL,
    [UserId] INT          NOT NULL,
    [Time]   NVARCHAR (5) NOT NULL,
    CONSTRAINT [PK_NotificationTimes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_NotificationTimes_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_NotificationTimes]
    ON [dbo].[NotificationTimes]([UserId] ASC, [Time] ASC);

