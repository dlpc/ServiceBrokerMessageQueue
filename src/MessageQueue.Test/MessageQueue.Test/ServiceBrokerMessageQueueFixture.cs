using System.Globalization;
using NUnit.Framework;

namespace MessageQueue.Test
{
    [TestFixture]
    public class ServiceBrokerMessageQueueFixture : RollbackFixture
    {
        private const string TestQueue = "test_queue";

        [Test]
        public void WriteToQueue()
        {
            const string sentMessage = "<message>Message</message>";
            var qMgr = new QueueManager(@".\SQLI03",@"Test_SMO_Database");
          
            qMgr.CreateQueue(TestQueue);
            var mq = qMgr.OpenQueue(TestQueue);
            mq.Send(sentMessage);

            var receivedMessage = mq.Receive();

            Assert.That(sentMessage.ToString(CultureInfo.CurrentCulture),Is.EqualTo(receivedMessage.ToString(CultureInfo.CurrentCulture)));
        }
    }
}
