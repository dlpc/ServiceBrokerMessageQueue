IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[message_queue].[create_queue]')
                    AND type IN ( N'P', N'PC' ) ) 
BEGIN
    DROP PROCEDURE [message_queue].[create_queue];
END ;
GO

CREATE PROCEDURE [message_queue].[create_queue]
	@queue_name nvarchar(50) 
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

        -- Determine Service Names

    DECLARE @target_queue_name nvarchar(100);
    DECLARE @initiator_queue_name nvarchar(100);
    DECLARE @target_service_name nvarchar(100);
    DECLARE @initiator_service_name nvarchar(100);
    DECLARE @message_type_name nvarchar(100);
    DECLARE @contract_name nvarchar(100);

    
    DECLARE @SQLString nvarchar(500);
    DECLARE @ParmDefinition nvarchar(500);

    SET @message_type_name  = @queue_name + '_message'
    SET @SQLString = N'CREATE MESSAGE TYPE ' + @message_type_name +
    ' VALIDATION = WELL_FORMED_XML ;'
    EXECUTE sp_executesql @SQLString
    
    SET @contract_name  = @queue_name + '_contract'
    SET @SQLString = 'CREATE CONTRACT ' + @contract_name + '(' + @message_type_name + ' SENT BY INITIATOR);'
    EXECUTE sp_executesql @SQLString


    SET @initiator_queue_name  = @queue_name + '_initiator'
    SET @SQLString =
     N'CREATE QUEUE [message_queue].['+ @initiator_queue_name + '];';
    EXECUTE sp_executesql @SQLString

    SET @target_queue_name = @queue_name 
    SET @SQLString =
     N'CREATE QUEUE [message_queue].['+ @target_queue_name + '];';
    EXECUTE sp_executesql @SQLString


    SET @initiator_service_name  = @queue_name + '_initiator_service'
	SET @SQLString =
     N'CREATE SERVICE ['+ @initiator_service_name + '] ON QUEUE [message_queue].[' + @initiator_queue_name +'];';
    EXECUTE sp_executesql @SQLString
    
    SET @target_service_name  = @queue_name + '_service'
    SET @SQLString =
     N'CREATE SERVICE ['+ @target_service_name + '] ON QUEUE [message_queue].[' + @target_queue_name +'] (' + @contract_name + ');';
    EXECUTE sp_executesql @SQLString

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