IF NOT EXISTS (SELECT schema_name 
    FROM information_schema.schemata 
    WHERE schema_name = 'message_queue' )
BEGIN
    EXEC sp_executesql N'CREATE SCHEMA message_queue;';
END