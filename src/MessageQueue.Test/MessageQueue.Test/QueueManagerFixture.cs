using System.Transactions;
using Common;
using MessageQueue.Exception;
using NUnit.Framework;

namespace MessageQueue.Test
{
    [TestFixture]
    public class QueueManagerFixture
    {
        private const string Server = @".\SQLI03";
        private const string Database = "Test_SMO_Database";
        private const string _queueName = "test_queue";
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


        [Test]
        public void CreateQueue_CreatesNamedQueue()
        {
            var qmgr = new QueueManager(Server,Database);
            qmgr.CreateQueue(_queueName);

            Assert.That(DatabaseVerification.CheckSysObjectExists("message_queue", _queueName, "SERVICE_QUEUE"),Is.True);
        }

        [Test]
        public void OpenQueue_OpensNamedMessageQueue()
        {
            var qmgr = new QueueManager(Server, Database);
            qmgr.CreateQueue(_queueName);
            var q = qmgr.OpenQueue(_queueName);

            Assert.That(q.QueueName, Is.EqualTo(_queueName));
        }

        [Test]
        public void OpenQueue_ThrowsIfQueueDoesNotExist()
        {
            var qmgr = new QueueManager(Server, Database);
            Assert.Throws<QueueNotFoundException>(() => qmgr.OpenQueue("non_existant_test_queue"));
        }

    }
}
