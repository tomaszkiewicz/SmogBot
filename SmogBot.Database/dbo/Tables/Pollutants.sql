CREATE TABLE [dbo].[Pollutants] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (128) NOT NULL,
    [Norm] DECIMAL (9, 3) NOT NULL,
    [Unit] NVARCHAR (10)  NOT NULL,
    CONSTRAINT [PK_Pollutants] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Pollutants]
    ON [dbo].[Pollutants]([Name] ASC);

