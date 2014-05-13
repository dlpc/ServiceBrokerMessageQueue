using System.Transactions;
using Common;
using MessageQueue.Exception;
using NUnit.Framework;

namespace MessageQueue.Test
{
    [TestFixture]
    public class QueueManagerFixture
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


        [Test]
        public void CreateQueue_CreatesNamedQueue()
        {
            var qmgr = new QueueManager(@".\SQLI03","Test_SMO_Database");
            qmgr.CreateQueue("test_queue");

            Assert.That(DatabaseVerification.CheckSysObjectExists("message_queue", "test_queue", "SERVICE_QUEUE"),Is.True);
        }

        [Test]
        [Ignore]
        public void OpenQueue_OpensNamedMessageQueue()
        {
            var qmgr = new QueueManager(@".\SQLI03", "Test_SMO_Database");
            qmgr.CreateQueue("test_queue");            
        }

        [Test]
        public void OpenQueue_ThrowsIfQueueDoesNotExist()
        {
            var qmgr = new QueueManager(@".\SQLI03", "Test_SMO_Database");
            Assert.Throws<QueueNotFoundException>(() => qmgr.OpenQueue("non_existant_test_queue"));
        }

    }
}
