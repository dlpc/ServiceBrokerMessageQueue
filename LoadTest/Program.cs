using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Messaging;
using System.Threading;
using System.Transactions;
using MessageQueue;

namespace LoadTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //parse args

            //check database

            //check message queue
            string messageQueueName = args[2];

            const int value = 9;

            for (int i = 0; i < value; i++)
            {
                var thread = new Thread(() => WriteToMsmqAndDatabase(messageQueueName, value));
                thread.Start();
            }

            for (int i = 0; i < value; i++)
            {
                var thread = new Thread(() => WriteToDatabase(messageQueueName, value));
                thread.Start();
            }
            for (int i = 0; i < value; i++)
            {
                var thread = new Thread(() => WriteSbmqAndDatabase(messageQueueName, value));
                thread.Start();
            }
        }


        private static void WriteSbmqAndDatabase(string messageQueueName, int value)
        {
            var sw = new Stopwatch();
            sw.Start();

            const int numberOfExecutions = 10000;
            for (int i = 0; i < numberOfExecutions; i++)
            {
                WriteToSbQueueAndDatabase(messageQueueName, value);
            }
            sw.Stop();

            Console.WriteLine("{0} entries written to SBMQ and Database in {1} ms", numberOfExecutions,
                sw.ElapsedMilliseconds);
        }

        private static void WriteToMsmqAndDatabase(string messageQueueName, int value)
        {
            var sw = new Stopwatch();
            sw.Start();

            const int numberOfExecutions = 10000;
            for (int i = 0; i < numberOfExecutions; i++)
            {
                WriteToQueueAndDatabase(messageQueueName, value);
            }
            sw.Stop();

            Console.WriteLine("{0} entries written to MSMQ and Database in {1} ms", numberOfExecutions,
                sw.ElapsedMilliseconds);
        }

        private static void WriteToDatabase(string messageQueueName, int value)
        {
            var sw = new Stopwatch();
            sw.Start();

            const int numberOfExecutions = 10000;
            for (int i = 0; i < numberOfExecutions; i++)
            {
                WriteToDatabase(value);
            }
            sw.Stop();

            Console.WriteLine("{0} entries written to Database in {1} ms", numberOfExecutions,
                sw.ElapsedMilliseconds);
        }


        private static void WriteToQueueAndDatabase(string messageQueueName, int value)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                string msmq = string.Format(@".\private$\{0}", messageQueueName);
                var messageQueue = new System.Messaging.MessageQueue(msmq);
                messageQueue.Send("sds", MessageQueueTransactionType.Automatic);

                using (var connection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["DB"].ToString()))
                {
                    connection.Open();
                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = @"Insert into test_table            ([key]
                    ,[data])values (@key,@data)"
                    };

                    cmd.Parameters.AddWithValue("key", Guid.NewGuid().ToString());
                    cmd.Parameters.AddWithValue("data", value);

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    scope.Complete();
                }
            }
        }

        private static void WriteToSbQueueAndDatabase(string messageQueueName, int value)
        {
            var qm = new QueueManager(ConfigurationManager
                .ConnectionStrings["DB"].ToString());
            MessageQueue.MessageQueue q = qm.OpenQueue(messageQueueName);

            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                using (var connection = new SqlConnection(ConfigurationManager
                    .ConnectionStrings["DB"].ToString()))
                {
                    q.Send("<message>Message</message>");


                    connection.Open();
                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = @"Insert into test_table            ([key]
                    ,[data])values (@key,@data)"
                    };

                    cmd.Parameters.AddWithValue("key", Guid.NewGuid().ToString());
                    cmd.Parameters.AddWithValue("data", value);

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                 
                scope.Complete();
                }
            }
        }


        private static void WriteToDatabase(int value)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                using (var connection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["DB"].ToString()))
                {
                    connection.Open();
                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = @"Insert into test_table            ([key]
                    ,[data])values (@key,@data)"
                    };

                    cmd.Parameters.AddWithValue("key", Guid.NewGuid().ToString());
                    cmd.Parameters.AddWithValue("data", value);

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    cmd.Parameters["key"].Value = Guid.NewGuid().ToString();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    scope.Complete();
                }
            }
        }
    }
}