using Common;
using MessageQueue.Exception;
using NUnit.Framework;

namespace MessageQueue.Test
{
    [TestFixture]
    public class QueueManagerFixture : RollbackFixture
    {
        public const string QueueName = "test_queue";
            
        [Test]
        public void CreateQueue_CreatesNamedQueue()
        {
            var qmgr = GetQueueManager();
            qmgr.CreateQueue(QueueName);

            Assert.That(DatabaseVerification.CheckSysObjectExists("message_queue", QueueName, "SERVICE_QUEUE", 
                DatabaseConnection.CreateSqlConnection(TestDatabaseSettings.Server, TestDatabaseSettings.Database)),Is.True);
        }

        [Test]
        public void DeleteQueue_DeletesNamedQueue()
        {
            var qmgr = GetQueueManager();
            qmgr.CreateQueue(QueueName);
            Assert.That(DatabaseVerification.CheckSysObjectExists("message_queue", QueueName, "SERVICE_QUEUE", 
                DatabaseConnection.CreateSqlConnection(TestDatabaseSettings.Server, TestDatabaseSettings.Database)), Is.True);
 
            qmgr.DeleteQueue(QueueName);
            Assert.That(DatabaseVerification.CheckSysObjectExists("message_queue", QueueName, "SERVICE_QUEUE", 
                DatabaseConnection.CreateSqlConnection(TestDatabaseSettings.Server, TestDatabaseSettings.Database)), Is.False);
        }

        [Test]
        public void OpenQueue_OpensNamedMessageQueue()
        {
            var qmgr = GetQueueManager();
            qmgr.CreateQueue(QueueName);
            var q = qmgr.OpenQueue(QueueName);

            Assert.That(q.QueueName, Is.EqualTo(QueueName));
        }

        [Test]
        public void OpenQueue_Throws_IfQueueDoesNotExist()
        {
            var qmgr = GetQueueManager();
            Assert.Throws<QueueNotFoundException>(() => qmgr.OpenQueue("non_existant_test_queue"));
        }

        [Test]
        public void DoesQueueExist_ReturnsFalse_IfQueueDoesNotExist()
        {
            var qmgr = GetQueueManager();

            var result = qmgr.QueueExists("non_existant_test_queue");

            Assert.That(result, Is.False);
        }

        [Test]
        public void DoesQueueExist_ReturnesTrue_IfQueueDoesExist()
        {
            var qmgr = GetQueueManager();
            qmgr.CreateQueue(QueueName);
            var result = qmgr.QueueExists(QueueName);

            Assert.That(result, Is.True);
        }

        [Test]
        public void GetQueues_ReturnsEmptyList_IfNoQueues()
        {
            var qmgr = GetQueueManager();

            var queues =  qmgr.GetQueues();

            Assert.That(queues.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetQueues_ReturnsOneQueue_IfQueueExists()
        {
            var qmgr = GetQueueManager();
            qmgr.CreateQueue(QueueName);

            var queues = qmgr.GetQueues();

            Assert.That(queues.Count, Is.EqualTo(1));
            Assert.That(queues[0].QueueName, Is.EqualTo(QueueName));
        }

        private static QueueManager GetQueueManager()
        {
            var qmgr = new QueueManager(TestDatabaseSettings.Server, TestDatabaseSettings.Database);
            return qmgr;
        }


    }
}
