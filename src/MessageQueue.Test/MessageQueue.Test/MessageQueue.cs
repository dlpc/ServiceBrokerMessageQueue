using System;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;

namespace MessageQueue.Test
{
    [TestFixture]
    public class MessageQueue
    {

        [Test]
        public void WriteToQueue()
        {
            var connection = new SqlConnection(@"Server=.\SQLI03;Database=Test_SMO_Database;Trusted_Connection=True;");
            Send(connection);

            Receive(connection);
        }

        private static void Receive(SqlConnection connection)
        {
            string SQL = string.Format(@" 
            waitfor(  
                RECEIVE top (@count) conversation_handle,service_name,message_type_name,message_body,message_sequence_number  
                FROM [{0}]  
                    ), timeout @timeout", "TargetQueue");
            SqlCommand cmd = new SqlCommand(SQL, connection);

            SqlParameter pCount = cmd.Parameters.Add("@count", SqlDbType.Int);
            pCount.Value = 1;
            var timeout = TimeSpan.FromMilliseconds(500);
            SqlParameter pTimeout = cmd.Parameters.Add("@timeout", SqlDbType.Int);

            if (timeout == TimeSpan.MaxValue)
            {
                pTimeout.Value = -1;
            }
            else
            {
                pTimeout.Value = (int) timeout.TotalMilliseconds;
            }

            cmd.CommandTimeout = 0; //honor the RECIEVE timeout, whatever it is. 


            var result = cmd.ExecuteReader();
        }

        private static void Send(SqlConnection connection)
        {
            const string message = "<message>Message</message>";
            const string sendCommandTemplate = @"DECLARE @message XML ;
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

            var sendCommand = string.Format(sendCommandTemplate, message);

            var command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = sendCommand;
            command.CommandType = CommandType.Text;

            connection.Open();
            command.ExecuteScalar();
        }
    }
}
