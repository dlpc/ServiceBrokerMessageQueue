using System;
using System.Data;
using System.Data.SqlClient;

namespace MessageQueue
{
    internal class ServiceBrokerMessageQueue : MessageQueue, IDisposable
    {
        private readonly string _connectionString;


        public ServiceBrokerMessageQueue(string server, string database, string queueName)
        {
            _connectionString = Common.DatabaseConnection.CreateConnectionString(server, database);
            QueueName = queueName;
        }

        public ServiceBrokerMessageQueue(string connectionString, string queueName)
        {
            _connectionString = connectionString;
            QueueName = queueName;
        }


        public void Dispose()
        {
        }

        public string QueueName { get; private set; }

        public void Send(string message)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                const string sendCommandTemplate = @"DECLARE @message XML;
        	    SET @message = N'{0}' ;

	           
	            BEGIN DIALOG CONVERSATION @conversationHandle
		            FROM SERVICE {1}
		            TO SERVICE '{2}'
                    
		            ON CONTRACT [{3}]
		            WITH ENCRYPTION = OFF;

	            SEND ON CONVERSATION @conversationHandle  
                MESSAGE TYPE {4}
                 (@message);";

                string sendCommand = String.Format(sendCommandTemplate, message,
                    QueueNameConvention.GetInitiatorServiceName(QueueName),
                    QueueNameConvention.GetTargetServiceName(QueueName),
                    QueueName + "_contract", QueueName + "_message");

                var cmd = new SqlCommand
                {
                    Connection = connection,
                    CommandText = sendCommand,
                    CommandType = CommandType.Text
                };

                SqlParameter pCount = cmd.Parameters.Add("@conversationHandle", SqlDbType.UniqueIdentifier);
                pCount.Value = Guid.NewGuid();

//                cmd.Parameters.Add(pCount);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                connection.Close();
            }

        }

        public string Receive()
        {
            string msg;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = String.Format(@"waitfor(  
                RECEIVE top (@count) conversation_handle,service_name,message_type_name, CAST(message_body AS XML) as msg,message_sequence_number  
                FROM [{0}]), timeout @timeout",
                    "message_queue].[" + QueueNameConvention.GetTargetQueueName(QueueName) + "");
                var cmd = new SqlCommand(sql, connection);

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
                msg = result["msg"].ToString();
                connection.Close();
            }

            return msg;
        }
    }
}