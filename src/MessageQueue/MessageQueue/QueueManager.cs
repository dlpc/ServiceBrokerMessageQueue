using System.Data;
using System.Data.SqlClient;

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
            return null;    
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
