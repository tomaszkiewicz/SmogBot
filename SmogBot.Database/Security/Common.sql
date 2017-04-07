CREATE SCHEMA [Common]
    AUTHORIZATION [dbo];






GO
GRANT SELECT
    ON SCHEMA::[Common] TO [Notifier];


GO
GRANT EXECUTE
    ON SCHEMA::[Common] TO [Notifier];


GO
GRANT SELECT
    ON SCHEMA::[Common] TO [Bot];


GO
GRANT EXECUTE
    ON SCHEMA::[Common] TO [Bot];

