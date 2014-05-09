CREATE PROCEDURE message_queue.create_queue
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

    DECLARE @full_queue_name nvarchar(100);
    DECLARE @SQLString nvarchar(500);
    DECLARE @ParmDefinition nvarchar(500);


    SET @full_queue_name = @queue_name + '_initiator'
    SET @SQLString =
     N'CREATE QUEUE [message_queue].['+ @full_queue_name + '];';
    EXECUTE sp_executesql @SQLString



        -- Check if Service Names Exist 
        
        -- Determine Service Names

        -- Check if Service Names Exist

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