CREATE TABLE [dbo].[Feedback] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      INT            NOT NULL,
    [CreatedTime] DATETIME       CONSTRAINT [DF_Feedback_CreatedTime] DEFAULT (getutcdate()) NOT NULL,
    [Message]     NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Feedback] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Feedback_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

