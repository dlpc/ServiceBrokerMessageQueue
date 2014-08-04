using System.Globalization;
using System.Transactions;
using NUnit.Framework;

namespace MessageQueue.Test
{
    [TestFixture]
    public class ServiceBrokerMessageQueueFixture 
    {
        private const string TestQueue = "test_queue";

        [Test]
        [Ignore]
        public void WriteToQueue()
        {
            const string sentMessage = "<message>Message</message>";
            var qMgr = new QueueManager(@".\SQLI03",@"Test_SMO_Database");
          
            qMgr.CreateQueue(TestQueue);
            var mq = qMgr.OpenQueue(TestQueue);

            using (var scope = new TransactionScope())
            {
                mq.Send(sentMessage);
                scope.Complete();
                
            }


            var receivedMessage = mq.Receive();

            Assert.That(sentMessage.ToString(CultureInfo.CurrentCulture),Is.EqualTo(receivedMessage.ToString(CultureInfo.CurrentCulture)));
        }
    }
}
