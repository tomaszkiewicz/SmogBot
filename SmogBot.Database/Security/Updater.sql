CREATE SCHEMA [Updater]
    AUTHORIZATION [dbo];



GO

CREATE LOGIN [Updater] WITH PASSWORD = 'invalidPassword';
GO

CREATE USER [Updater] FOR LOGIN [Updater]
    WITH DEFAULT_SCHEMA = [Updater];


GO

GRANT SELECT
    ON SCHEMA::[Updater] TO [Updater];

GO
GRANT EXECUTE
    ON SCHEMA::[Updater] TO [Updater];

