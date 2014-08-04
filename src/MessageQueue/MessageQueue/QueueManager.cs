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
            _connectionString = string.Empty;
        }

        public QueueManager(string connectionString)
        {
            _connectionString = connectionString;

        }

        public string Server { get; private set; }
        public string Database { get; private set; }
        private readonly string _connectionString; 

        public MessageQueue OpenQueue(string queueName)
        {
            CheckIfMessageQueueExists(queueName);

            if (string.IsNullOrEmpty(_connectionString))
                return new ServiceBrokerMessageQueue(Server,Database,queueName);

            return new ServiceBrokerMessageQueue(_connectionString,queueName);
        }

        private void CheckIfMessageQueueExists(string queueName)
        {
            var doesQueueExist = DatabaseVerification.CheckSysObjectExists("message_queue", queueName, "SERVICE_QUEUE", Common.DatabaseConnection.CreateSqlConnection(_connectionString));

            if (doesQueueExist) return;

            const string message = "Queue {0} cannot be opened, it does not exist";
            throw new QueueNotFoundException(string.Format(message, queueName));
        }

        public void CreateQueue(string queueName)
        {
            var sqlConnection = Common.DatabaseConnection.CreateSqlConnection(_connectionString);
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
