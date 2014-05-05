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

            const string sendCommand = @"DECLARE @message XML ;
        	SET @message = N'<message>Hello, World!</message>' ;

	        -- Declare a variable to hold the conversation
	        -- handle.

	        DECLARE @conversationHandle UNIQUEIDENTIFIER ;

	        -- Begin the dialog.

	        BEGIN DIALOG CONVERSATION @conversationHandle
		        FROM SERVICE InitiatorService
		        TO SERVICE 'TargetService'
		        ON CONTRACT HelloWorldContract 
		        WITH ENCRYPTION = OFF;

        	-- Send the message on the dialog.

	        SEND ON CONVERSATION @conversationHandle
	        MESSAGE TYPE HelloWorldMessage
	        (@message) ;";

            SqlCommand command = new SqlCommand();
            var connection = new SqlConnection(@"Server=.\SQLI03;Database=Test_SMO_Database;Trusted_Connection=True;");
            command.Connection = connection;
            command.CommandText = sendCommand;
            command.CommandType = CommandType.Text;

            // Add the input parameter and set its properties.

            // Add the parameter to the Parameters collection. 

            // Open the connection and execute the reader.
            connection.Open();
            command.ExecuteScalar();

        }

    }
}
