namespace MessageQueue
{
    public interface MessageQueue
    {
        void Send(object message);
        object Receive();
    }

    
}
