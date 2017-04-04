CREATE TABLE [dbo].[UsersPreferences] (
    [UserId] INT NOT NULL,
    [CityId] INT NULL,
    CONSTRAINT [PK_UsersPreferences] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_UsersPreferences_Cities] FOREIGN KEY ([CityId]) REFERENCES [dbo].[Cities] ([Id]),
    CONSTRAINT [FK_UsersPreferences_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);



