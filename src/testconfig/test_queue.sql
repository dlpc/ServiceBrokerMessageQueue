IF EXISTS (SELECT *
           FROM sys.services
           WHERE name = 'InitiatorService')
BEGIN
    DROP SERVICE InitiatorService ;
END ;
GO


IF EXISTS (SELECT *
           FROM sys.services
           WHERE name = 'TargetService')
BEGIN
    DROP SERVICE TargetService ;
END ;
GO


IF EXISTS (SELECT *
           FROM sys.service_contracts
           WHERE name = 'HelloWorldContract')
BEGIN
    DROP CONTRACT HelloWorldContract ;
END ;
GO



IF EXISTS (SELECT *
           FROM sys.service_message_types
           WHERE name = 'HelloWorldMessage')
BEGIN
    DROP MESSAGE TYPE HelloWorldMessage ;
END ;
GO




IF OBJECT_ID('[dbo].[InitiatorQueue]') IS NOT NULL AND
   EXISTS(SELECT *
          FROM sys.objects
          WHERE object_id = OBJECT_ID('[dbo].[InitiatorQueue]')
            AND type = 'SQ')
BEGIN
    DROP QUEUE [dbo].[InitiatorQueue] ;
END ;
GO


IF OBJECT_ID('[dbo].[TargetQueue]') IS NOT NULL AND
   EXISTS(SELECT *
          FROM sys.objects
          WHERE object_id = OBJECT_ID('[dbo].[TargetQueue]')
            AND type = 'SQ')
BEGIN
    DROP QUEUE [dbo].[TargetQueue] ;
END ;
GO
--------------------------------------------------------------------
