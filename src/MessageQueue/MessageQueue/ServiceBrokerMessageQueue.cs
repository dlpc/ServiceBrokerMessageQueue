using System;
using System.Data;
using System.Data.SqlClient;

namespace MessageQueue
{
    internal class ServiceBrokerMessageQueue : MessageQueue,IDisposable
    {
        private readonly SqlConnection _connection;

        public ServiceBrokerMessageQueue(SqlConnection connection)
        {
            _connection = connection;
            _connection.Open();
            //check that the queue exists ?
        }

        public void Dispose()
        {
            _connection.Close();
        }

        public void Send(string message)
        {
            const string sendCommandTemplate = @"DECLARE @message XML;
        	SET @message = N'{0}' ;

	        DECLARE @conversationHandle UNIQUEIDENTIFIER ;

	        BEGIN DIALOG CONVERSATION @conversationHandle
		        FROM SERVICE InitiatorService
		        TO SERVICE 'TargetService'
		        ON CONTRACT HelloWorldContract 
		        WITH ENCRYPTION = OFF;

	        SEND ON CONVERSATION @conversationHandle
	        MESSAGE TYPE HelloWorldMessage
	        (@message) ;";

            var sendCommand = String.Format(sendCommandTemplate, message);

            var command = new SqlCommand
            {
                Connection = _connection,
                CommandText = sendCommand,
                CommandType = CommandType.Text
            };

            command.ExecuteScalar();
        }

        public string Receive()
        {
            var sql = String.Format(@"waitfor(  
                RECEIVE top (@count) conversation_handle,service_name,message_type_name, CAST(message_body AS XML) as msg,message_sequence_number  
                FROM [{0}]), timeout @timeout", "TargetQueue");
            var cmd = new SqlCommand(sql, _connection);

            var pCount = cmd.Parameters.Add("@count", SqlDbType.Int);
            pCount.Value = 1;
            var timeout = TimeSpan.FromMilliseconds(500);
            var pTimeout = cmd.Parameters.Add("@timeout", SqlDbType.Int);

            if (timeout == TimeSpan.MaxValue)
            {
                pTimeout.Value = -1;
            }
            else
            {
                pTimeout.Value = (int) timeout.TotalMilliseconds;
            }

            cmd.CommandTimeout = 0;
            
            
            SqlDataReader result = cmd.ExecuteReader();
            result.Read();
            return (string)result["msg"];
        }
    }
}
