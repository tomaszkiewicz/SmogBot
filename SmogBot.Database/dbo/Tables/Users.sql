CREATE TABLE [dbo].[Users] (
    [Id]                    INT             IDENTITY (1, 1) NOT NULL,
    [ChannelId]             NVARCHAR (128)  NOT NULL,
    [FromId]                NVARCHAR (128)  NOT NULL,
    [FromName]              NVARCHAR (128)  NOT NULL,
    [ConversationId]        NVARCHAR (128)  NOT NULL,
    [CreatedTime]           DATETIME        CONSTRAINT [DF_Users_CreatedTime] DEFAULT (getutcdate()) NOT NULL,
    [LastActivityTime]      DATETIME        CONSTRAINT [DF_Users_LastActivityTime] DEFAULT (getutcdate()) NOT NULL,
    [ConversationReference] NVARCHAR (1024) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);










GO
CREATE NONCLUSTERED INDEX [IX_Users]
    ON [dbo].[Users]([ConversationId] ASC);

