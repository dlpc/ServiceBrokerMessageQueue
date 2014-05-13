using System.Globalization;
using Common.Test;
using NUnit.Framework;

namespace MessageQueue.Test
{
    [TestFixture]
    public class ServiceBrokerMessageQueueFixture
    {
        [Test]
        public void WriteToQueue()
        {
            var connection = DatabaseConnection.CreateSqlConnection(@".\SQLI03", @"Test_SMO_Database");
            const string sentMessage = "<message>Message</message>";
            var mq = new ServiceBrokerMessageQueue(connection,"test_queue");
            mq.Send(sentMessage);

            var receivedMessage = mq.Receive();

            Assert.That(sentMessage.ToString(CultureInfo.CurrentCulture),Is.EqualTo(receivedMessage.ToString(CultureInfo.CurrentCulture)));
        }
    }
}
