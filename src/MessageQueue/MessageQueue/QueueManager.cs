namespace MessageQueue
{
    public class QueueManager
    {
        public QueueManager()
        {
        }

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
            
        }
    }
}
