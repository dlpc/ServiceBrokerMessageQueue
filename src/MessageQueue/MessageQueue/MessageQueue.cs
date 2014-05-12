namespace MessageQueue
{
    public interface MessageQueue
    {
        void Send(string message);
        string Receive();
    }
}
