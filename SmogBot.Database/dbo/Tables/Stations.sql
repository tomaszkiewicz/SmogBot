CREATE TABLE [dbo].[Stations] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [CityId] INT            NOT NULL,
    [Name]   NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_Stations] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Stations_Cities] FOREIGN KEY ([CityId]) REFERENCES [dbo].[Cities] ([Id])
);






GO
CREATE NONCLUSTERED INDEX [IX_Stations]
    ON [dbo].[Stations]([CityId] ASC, [Name] ASC);

