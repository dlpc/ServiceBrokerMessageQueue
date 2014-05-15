using System.Data;
using System.Data.SqlClient;
using Common;
using MessageQueue.Exception;

namespace MessageQueue
{
    public class QueueManager
    {
        public QueueManager(string server, string database)
        {
            Server = server;
            Database = database;
        }

        public string Server { get; private set; }
        public string Database { get; private set; }

        public MessageQueue OpenQueue(string queueName)
        {
            CheckIfMessageQueueExists(queueName);

            return new ServiceBrokerMessageQueue(DatabaseConnection.CreateSqlConnection(Server,Database),queueName);
        }

        private static void CheckIfMessageQueueExists(string queueName)
        {
            bool doesQueueExist = DatabaseVerification.CheckSysObjectExists("message_queue", queueName, "SERVICE_QUEUE");

            if (!doesQueueExist)
            {
                const string message = "Queue {0} cannot be opened, it does not exist";
                throw new QueueNotFoundException(string.Format(message, queueName));
            }
        }

        public void CreateQueue(string queueName)
        {
            var sqlConnection = DatabaseConnection.CreateSqlConnection(@".\SQLI03", @"Test_SMO_Database");
            sqlConnection.Open();

            var cmd = new SqlCommand("message_queue.create_queue", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            var procNameParam = cmd.Parameters.Add("@queue_name", SqlDbType.NVarChar);
            procNameParam.Value = queueName;

            cmd.ExecuteNonQuery();

            
        }

    }
}
