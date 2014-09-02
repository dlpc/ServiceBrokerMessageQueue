using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Common;
using MessageQueue.Exception;

namespace MessageQueue
{
    public class QueueManager
    {
        private readonly string _connectionString;

        public QueueManager(string server, string database)
        {
            Server = server;
            Database = database;
            _connectionString = DatabaseConnection.CreateConnectionString(server, database);
        }

        public QueueManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string Server { get; private set; }
        public string Database { get; private set; }

        public MessageQueue OpenQueue(string queueName)
        {
            CheckIfMessageQueueExists(queueName);

            if (string.IsNullOrEmpty(_connectionString))
                return new ServiceBrokerMessageQueue(Server, Database, queueName);

            return new ServiceBrokerMessageQueue(_connectionString, queueName);
        }

        private void CheckIfMessageQueueExists(string queueName)
        {
            bool doesQueueExist = DatabaseVerification.CheckSysObjectExists("message_queue", queueName, "SERVICE_QUEUE",
                DatabaseConnection.CreateSqlConnection(_connectionString));

            if (doesQueueExist) return;

            const string message = "Queue {0} cannot be opened, it does not exist";
            throw new QueueNotFoundException(string.Format(message, queueName));
        }

        public void CreateQueue(string queueName)
        {
            SqlConnection sqlConnection = DatabaseConnection.CreateSqlConnection(_connectionString);
            sqlConnection.Open();

            var cmd = new SqlCommand("message_queue.create_queue", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            SqlParameter procNameParam = cmd.Parameters.Add("@queue_name", SqlDbType.NVarChar);
            procNameParam.Value = queueName;

            cmd.ExecuteNonQuery();
        }

        public void DeleteQueue(string queueName)
        {
            SqlConnection sqlConnection = DatabaseConnection.CreateSqlConnection(_connectionString);
            sqlConnection.Open();

            var cmd = new SqlCommand("message_queue.delete_queue", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            SqlParameter procNameParam = cmd.Parameters.Add("@queue_name", SqlDbType.NVarChar);
            procNameParam.Value = queueName;

            cmd.ExecuteNonQuery();
        }

        public bool QueueExists(string queueName)
        {
            return DatabaseVerification.CheckSysObjectExists("message_queue", queueName, "SERVICE_QUEUE",
                DatabaseConnection.CreateSqlConnection(_connectionString));
        }

        public List<MessageQueue> GetQueues()
        {
            var queues = new List<MessageQueue>();

            using (SqlConnection sqlConnection = DatabaseConnection.CreateSqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var cmd = new SqlCommand("[message_queue].[get_list_of_queues]", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlDataReader result = cmd.ExecuteReader();

                while (result.Read())
                {
                    string queueName = result["queue_name"].ToString();

                    if (!queueName.EndsWith("_initiator"))
                    {
                        var queue = new ServiceBrokerMessageQueue(_connectionString, queueName);
                        queues.Add(queue);
                    }
                }

                sqlConnection.Close();
            }

            return queues;
        }
    }
}