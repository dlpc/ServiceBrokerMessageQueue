using System.Transactions;
using Common;
using MessageQueue.Exception;
using NUnit.Framework;

namespace MessageQueue.Test
{
    public class RollbackFixture
    {
        private TransactionScope _scope;

        [SetUp]
        public void Setup()
        {
            _scope = new TransactionScope(TransactionScopeOption.Required, new  TransactionOptions {IsolationLevel = IsolationLevel.ReadUncommitted} );
        }

        [TearDown]
        public void Teardown()
        {
            _scope.Dispose();
            _scope = null;
        }
    }

    [TestFixture]
    public class QueueManagerFixture : RollbackFixture
    {
        private const string Server = @".\SQLI03";
        private const string Database = "Test_SMO_Database";
        private const string QueueName = "test_queue";


        [Test]
        public void CreateQueue_CreatesNamedQueue()
        {
            var qmgr = new QueueManager(Server,Database);
            qmgr.CreateQueue(QueueName);

            Assert.That(DatabaseVerification.CheckSysObjectExists("message_queue", QueueName, "SERVICE_QUEUE"),Is.True);
        }

        [Test]
        public void OpenQueue_OpensNamedMessageQueue()
        {
            var qmgr = new QueueManager(Server, Database);
            qmgr.CreateQueue(QueueName);
            var q = qmgr.OpenQueue(QueueName);

            Assert.That(q.QueueName, Is.EqualTo(QueueName));
        }

        [Test]
        public void OpenQueue_ThrowsIfQueueDoesNotExist()
        {
            var qmgr = new QueueManager(Server, Database);
            Assert.Throws<QueueNotFoundException>(() => qmgr.OpenQueue("non_existant_test_queue"));
        }

    }
}
