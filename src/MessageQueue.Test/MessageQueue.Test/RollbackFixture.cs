using System.Transactions;
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
}