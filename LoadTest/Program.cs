using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Messaging;
using System.Threading;
using System.Transactions;

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

            const int value = 1;

            for (var i = 0; i < 10; i++)
            {
                var thread = new Thread(() => WriteToQueues(messageQueueName, value));
                thread.Start();
            }

            for (var i = 0; i < 10; i++)
            {
                var thread = new Thread(() => WriteToDatabase(messageQueueName, value));
                thread.Start();
            }


        }

        private static void WriteToQueues(string messageQueueName, int value)
        {
            var sw = new Stopwatch();
            sw.Start();

            const int numberOfExecutions = 1000;
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

            const int numberOfExecutions = 1000;
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
            using (var scope = new TransactionScope())
            {
                string msmq = string.Format(@".\private$\{0}", messageQueueName);
                var messageQueue = new MessageQueue(msmq);
                messageQueue.Send("sds", MessageQueueTransactionType.Automatic);

                var connection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["DB"].ToString());
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
                connection.Close();
                scope.Complete();
            }
        }

        private static void WriteToDatabase(int value)
        {
            using (var scope = new TransactionScope())
            {


                var connection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["DB"].ToString());
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

                cmd.Parameters["key"].Value = Guid.NewGuid().ToString();
                cmd.ExecuteNonQuery();
                connection.Close();
                scope.Complete();
            }
        }

    }
}