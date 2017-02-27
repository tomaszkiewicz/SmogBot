CREATE TABLE [dbo].[Users] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [ChannelId]      NVARCHAR (128) NOT NULL,
    [FromId]         NVARCHAR (128) NOT NULL,
    [FromName]       NVARCHAR (128) NOT NULL,
    [ConversationId] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

