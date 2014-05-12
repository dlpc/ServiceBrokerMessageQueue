﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.Text;
using Common.Test;
using NUnit.Framework;

namespace MessageQueue.Test
{
    [TestFixture]
    public class MessageQueue
    {

        [Test]
        public void WriteToQueue()
        {
            var connection = DatabaseConnection.CreateSqlConnection();
            const string sentMessage = "<message>Message</message>";
            Send(connection, sentMessage);
            
            string receivedMessage = Receive(connection);

            Assert.That(sentMessage.ToString(CultureInfo.CurrentCulture),Is.EqualTo(receivedMessage.ToString(CultureInfo.CurrentCulture)));
        }

        private static string Receive(SqlConnection connection)
        {
            string sql = string.Format(@" 
            waitfor(  
                RECEIVE top (@count) conversation_handle,service_name,message_type_name, CAST(message_body AS XML) as msg,message_sequence_number  
                FROM [{0}]), timeout @timeout", "TargetQueue");
            var cmd = new SqlCommand(sql, connection);

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

        private static void Send(SqlConnection connection, string message)
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

            var sendCommand = string.Format(sendCommandTemplate, message);

            var command = new SqlCommand
            {
                Connection = connection,
                CommandText = sendCommand,
                CommandType = CommandType.Text
            };

            connection.Open();
            command.ExecuteScalar();
        }
    }
}
