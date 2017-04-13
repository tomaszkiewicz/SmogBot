CREATE SCHEMA [Notifier]
    AUTHORIZATION [dbo];












GO

CREATE LOGIN [Notifier] WITH PASSWORD = 'invalidPassword';
GO

CREATE USER [Notifier] FOR LOGIN [Notifier]
    WITH DEFAULT_SCHEMA = [Notifier];


GO

GRANT SELECT
    ON SCHEMA::[Notifier] TO [Notifier];

GO
GRANT EXECUTE
    ON SCHEMA::[Notifier] TO [Notifier];



