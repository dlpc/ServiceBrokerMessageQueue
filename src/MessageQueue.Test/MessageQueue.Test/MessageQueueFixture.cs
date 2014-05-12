using System.Globalization;
using Common.Test;
using NUnit.Framework;

namespace MessageQueue.Test
{
    [TestFixture]
    public class MessageQueueFixture
    {
        [Test]
        public void WriteToQueue()
        {
            var connection = DatabaseConnection.CreateSqlConnection();
            const string sentMessage = "<message>Message</message>";
            var mq = new ServiceBrokerMessageQueue(connection);
            mq.Send(sentMessage);

            var receivedMessage = mq.Receive();

            Assert.That(sentMessage.ToString(CultureInfo.CurrentCulture),Is.EqualTo(receivedMessage.ToString(CultureInfo.CurrentCulture)));
        }
    }
}
