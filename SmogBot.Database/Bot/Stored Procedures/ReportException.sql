
CREATE PROCEDURE [Bot].[ReportException]
    @message NVARCHAR(MAX),
    @stackTrace NVARCHAR(MAX),
    @activity NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT  INTO [dbo].[Exceptions]
            (
             [CreateTime],
             [Message],
             [StackTrace],
             [Activity]
            )
    VALUES
            (
             GETUTCDATE(),
             @message,
             @stackTrace,
             @activity
            );
END;