using System;

namespace MessageQueue
{
    public interface MessageQueue:IDisposable  
    {
        string QueueName { get; }

        void Send(string message);
        string Receive();
    }
}
