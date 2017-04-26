CREATE TABLE [dbo].[Exceptions] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [CreateTime] DATETIME       CONSTRAINT [DF_Exceptions_CreateTime] DEFAULT (getutcdate()) NOT NULL,
    [Message]    NVARCHAR (MAX) NOT NULL,
    [StackTrace] NVARCHAR (MAX) NOT NULL,
    [Activity]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Exceptions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

