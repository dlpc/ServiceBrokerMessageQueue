IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[message_queue].[get_list_of_queues]')
                    AND type IN ( N'P', N'PC' ) ) 
BEGIN
    DROP PROCEDURE [message_queue].[get_list_of_queues];
END ;
GO

CREATE PROCEDURE [message_queue].[get_list_of_queues]
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT,
        QUOTED_IDENTIFIER,
        ANSI_NULLS,
        ANSI_PADDING,
        ANSI_WARNINGS,
        ARITHABORT,
        CONCAT_NULL_YIELDS_NULL ON;
    SET NUMERIC_ROUNDABORT OFF;
 
    DECLARE @localTran bit
    IF @@TRANCOUNT = 0
    BEGIN
        SET @localTran = 1
        BEGIN TRANSACTION LocalTran
    END
 
    BEGIN TRY

        SELECT sys.objects.name as queue_name FROM [sys].[objects] inner join sys.schemas 
        on sys.objects.schema_id = sys.schemas.schema_id
        where  [sys].[objects].[type_desc] = 'SERVICE_QUEUE' AND [sys].[schemas].[name] = 'message_queue'


 
    IF @localTran = 1 AND XACT_STATE() = 1
        COMMIT TRAN LocalTran
 
    END TRY
    BEGIN CATCH
 
        DECLARE @ErrorMessage NVARCHAR(4000)
        DECLARE @ErrorSeverity INT
        DECLARE @ErrorState INT
 
        SELECT  @ErrorMessage = ERROR_MESSAGE(),
                @ErrorSeverity = ERROR_SEVERITY(),
                @ErrorState = ERROR_STATE()
 
        IF @localTran = 1 AND XACT_STATE() <> 0
            ROLLBACK TRAN
 
        RAISERROR ( @ErrorMessage, @ErrorSeverity, @ErrorState)
 
    END CATCH
 
END