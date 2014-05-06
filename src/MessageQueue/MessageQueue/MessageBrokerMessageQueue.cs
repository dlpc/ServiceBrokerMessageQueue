using System;
using System.Data.SqlClient;

namespace MessageQueue
{
    public class MessageBrokerMessageQueue : MessageQueue,IDisposable
    {
        private readonly SqlConnection _connection;

        public MessageBrokerMessageQueue(SqlConnection connection)
        {
            _connection = connection;
            _connection.Open();
            //check that the queue exists
        }

        public void Send(object message)
        {
            throw new System.NotImplementedException();
        }

        public object Receive()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            _connection.Close();
            throw new NotImplementedException();
        }
    }
}
