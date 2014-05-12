using System;
using System.Data;
using System.Data.SqlClient;

namespace MessageQueue
{
    internal class ServiceBrokerMessageQueue : MessageQueue, IDisposable
    {
        private readonly SqlConnection _connection;
        private readonly string _queueName;

        public ServiceBrokerMessageQueue(SqlConnection connection, string queueName)
        {
            _connection = connection;
            _queueName = queueName;
        }

        public void Dispose()
        {
        }

        public void Send(string message)
        {
            try
            {
                _connection.Open();

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
	            (@message);";

                string sendCommand = String.Format(sendCommandTemplate, message);

                var cmd = new SqlCommand
                {
                    Connection = _connection,
                    CommandText = sendCommand,
                    CommandType = CommandType.Text
                };

//                SqlParameter pCount = cmd.Parameters.Add("@initiatorService", SqlDbType.NVarChar);
//                pCount.Value = QueueNameConvention.GetInitiatorServiceName(_queueName);

                cmd.ExecuteScalar();
            }
            finally
            {
                _connection.Close();
            }
        }

        public string Receive()
        {
            try
            {
                _connection.Open();

                string sql = String.Format(@"waitfor(  
                RECEIVE top (@count) conversation_handle,service_name,message_type_name, CAST(message_body AS XML) as msg,message_sequence_number  
                FROM [{0}]), timeout @timeout", "TargetQueue");
                var cmd = new SqlCommand(sql, _connection);

                SqlParameter pCount = cmd.Parameters.Add("@count", SqlDbType.Int);
                pCount.Value = 1;
                TimeSpan timeout = TimeSpan.FromMilliseconds(500);
                SqlParameter pTimeout = cmd.Parameters.Add("@timeout", SqlDbType.Int);

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
                return (string) result["msg"];
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}