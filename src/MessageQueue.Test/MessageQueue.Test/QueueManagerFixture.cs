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
        [Ignore]
        public void DeleteQueue_DeletesNamedQueue()
        {
            var qmgr = new QueueManager(TestDatabaseSettings.Server, TestDatabaseSettings.Database);
            qmgr.CreateQueue(QueueName);

            Assert.That(DatabaseVerification.CheckSysObjectExists("message_queue", QueueName, "SERVICE_QUEUE", 
                DatabaseConnection.CreateSqlConnection(TestDatabaseSettings.Server, TestDatabaseSettings.Database)), Is.True);
 


            qmgr.DeleteQueue(QueueName);
            Assert.That(DatabaseVerification.CheckSysObjectExists("message_queue", QueueName, "SERVICE_QUEUE", 
                DatabaseConnection.CreateSqlConnection(TestDatabaseSettings.Server, TestDatabaseSettings.Database)), Is.True);
 

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

    }
}
