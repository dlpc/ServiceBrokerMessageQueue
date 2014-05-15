namespace MessageQueue
{
    public interface MessageQueue
    {
        string QueueName { get; }

        void Send(string message);
        string Receive();
    }
}
