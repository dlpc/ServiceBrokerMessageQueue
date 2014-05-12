namespace MessageQueue
{
    internal static class QueueNameConvention
    {
        public static string GetInitiatorServiceName(string queueName)
        {
            return queueName + "_initiator_service";
        }

        public static string GetInitiatorQueueName(string queueName)
        {
            return queueName + "_initiator";
        }

        public static string GetTargetServiceName(string queueName)
        {
            return queueName + "_service";
        }


        public static string GetTargetQueueName(string queueName)
        {
            return queueName;
        }

    }

    
}
