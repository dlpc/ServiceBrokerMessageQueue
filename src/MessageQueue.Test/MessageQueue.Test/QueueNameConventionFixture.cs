using NUnit.Framework;

namespace MessageQueue.Test
{
    [TestFixture]
    public class QueueNameConventionFixture
    {
        private const string QueueName = "queueName";
        private const string InitiatorQueueSuffix = "_initiator";
        private const string InitiatorServiceSuffix = "_initiator_service";
        private const string TargetServiceSuffix = "_service";

        [Test]
        public void QueueInitiatorServiceIsNamedCorrectly()
        {
            Assert.That(QueueNameConvention.GetInitiatorServiceName(QueueName)
                , Is.EqualTo(QueueName + InitiatorServiceSuffix));
        }

        [Test]
        public void QueueInitiatorQueueIsNamedCorrectly()
        {
            Assert.That(QueueNameConvention.GetInitiatorQueueName(QueueName)
                , Is.EqualTo(QueueName + InitiatorQueueSuffix));
        }

        [Test]
        public void QueueTargetServiceIsNamedCorrectly()
        {
            Assert.That(QueueNameConvention.GetTargetServiceName(QueueName)
                , Is.EqualTo(QueueName + TargetServiceSuffix));
        }

        [Test]
        public void QueueTargetIsNamedCorrectly()
        {
            Assert.That(QueueNameConvention.GetTargetQueueName(QueueName)
                , Is.EqualTo(QueueName));
        }

    }
}
