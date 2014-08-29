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
            var qmgr = new QueueManager(TestDatabaseSettings.Server,TestDatabaseSettings.Database);
            qmgr.CreateQueue(QueueName);

            Assert.That(DatabaseVerification.CheckSysObjectExists("message_queue", QueueName, "SERVICE_QUEUE", 
                DatabaseConnection.CreateSqlConnection(TestDatabaseSettings.Server, TestDatabaseSettings.Database)),Is.True);
        }

        [Test]
        public void DeleteQueue_DeletesNamedQueue()
        {
            var qmgr = new QueueManager(TestDatabaseSettings.Server, TestDatabaseSettings.Database);
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
            var qmgr = new QueueManager(TestDatabaseSettings.Server, TestDatabaseSettings.Database);
            qmgr.CreateQueue(QueueName);
            var q = qmgr.OpenQueue(QueueName);

            Assert.That(q.QueueName, Is.EqualTo(QueueName));
        }

        [Test]
        public void OpenQueue_ThrowsIfQueueDoesNotExist()
        {
            var qmgr = new QueueManager(TestDatabaseSettings.Server, TestDatabaseSettings.Database);
            Assert.Throws<QueueNotFoundException>(() => qmgr.OpenQueue("non_existant_test_queue"));
        }

        [Test]
        public void DoesQueueExist_ReturnsFalseIfQueueDoesNotExist()
        {
            var qmgr = new QueueManager(TestDatabaseSettings.Server, TestDatabaseSettings.Database);

            var result = qmgr.QueueExists("non_existant_test_queue");

            Assert.That(result, Is.False);
        }

        [Test]
        public void DoesQueueExist_ReturnesTrueIfQueueDoesExist()
        {
            var qmgr = new QueueManager(TestDatabaseSettings.Server, TestDatabaseSettings.Database);
            qmgr.CreateQueue(QueueName);
            var result = qmgr.QueueExists(QueueName);

            Assert.That(result, Is.True);
        }

    }
}
